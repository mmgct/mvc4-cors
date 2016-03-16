function CallHome(baseUrl, method, simple) {
  if (typeof method === 'undefined') {
    method = 'TestCors';
  }
  if (typeof simple === 'undefined') {
    simple = true;
  }
  console.log("Calling " + method + " action");
  var jqxhr = $.ajax(
  {
    url: baseUrl + '/Home/' + method,
    method: simple ? 'GET' : 'PUT'
  }).done(function(resp, status, request) {
    var el = $("#response");
    var html;
    if (status === 'error') {
      html = "<div class='error'>Error - check Chrome dev tools or fiddler... cannot trap CORS errors in javascript</div>";
    } else {
      html = "<pre>";
      if (resp.length === 0) {
        html += "json: EMPTY";
      } else {
        html += "json: " + resp;
      }
      html += "</pre>";
      html += "<pre>";
      var acao = request.getResponseHeader("Access-Control-Allow-Origin");
      if (acao !== null) {
        html += "header: Access-Control-Allow-Origin: " + acao;
      } else {
        html += "header: Access-Control-Allow-Origin not present";
      }
    }
    el.fadeOut(500, function() {
      el.html(html);
      el.fadeIn(500);
    });
    console.log(resp);
    console.log(el);
  }).error(function(resp, status, error) {
    var el = $("#response");
    console.log(resp.state());
    var html = "<div class='alert alert-danger'>Error - check Chrome dev tools or fiddler... cannot trap specific CORS errors in javascript</div>";
    el.fadeOut(500, function() {
      el.html(html);
      el.fadeIn(500);
    });
    console.log(resp);
    console.log(error);
  });
}

function CallHomeBlocked(baseUrl) {
  return CallHome(baseUrl, "TestCorsBlocked");
}

function CallHomeInjected(baseUrl) {
  return CallHome(baseUrl, "TestCorsInjection");
}

function CallHomeComplex(baseUrl) {
  return CallHome(baseUrl, "TestCors", false);
}
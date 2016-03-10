function CallHome() {
  console.log("Calling TestCors action");
  var jqxhr = $.ajax('/Home/TestCors').done(function(resp) {
    $("#response").innerText = resp;
  });
}
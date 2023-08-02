requirejs.config({
  baseUrl: `${webEnvironment.baseUrl}/js`,
  paths: {
    'bootstrap': '../lib/bootstrap/dist/js/bootstrap.bundle',
    'jquery': '../lib/jquery/jquery',
    'jquery-validation': '../lib/jquery-validation/dist/jquery.validate',
    'jquery-validation-unobtrusive': '../lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive',
    'core-js': '../lib/core-js/shim',
    'whatwg-fetch': '../lib/whatwg-fetch/dist/fetch.umd',
    'suggestions-jquery': '../lib/suggestions-jquery/dist/js/jquery.suggestions'
  },
  shim: {
    'jquery': {
      exports: 'JQuery'
    },
    'jquery-validation-unobtrusive': {
      deps: [
        'jquery-validation'
      ]
    },
    'suggestions-jquery': {
      deps: [
        'jquery'
      ]
    }
  },
});

requirejs(['bootstrap', 'jquery', 'core-js', 'whatwg-fetch']);

document.cookie = '.JSTimeZoneOffset=' + new Date().getTimezoneOffset();

// Get the header
const header = document.getElementById("header");

// Get the offset position of the navbar
const sticky = header.offsetTop;

// Add the sticky class to the header when you reach its scroll position. Remove "sticky" when you leave the scroll position
function onScrollWindow() {
  if (window.pageYOffset >= sticky) {
    header.classList.add("sticky");
  } else {
    header.classList.remove("sticky");
  }
}

// When the user scrolls the page, execute myFunction
window.onscroll = function () { onScrollWindow() };
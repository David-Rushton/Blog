/**
 * in non-production environments warnings and errors are made more visible to the user
 */
(extendedLoggingInNonProudctionEnvironments = () => {

    const isProduction = window.location.href.startsWith('https://david-rushton.dev');
    const originalConsole = window.console;

    if( ! isProduction ) {

        console.warn = function(message) {
            originalConsole.apply(this, arguments);
            alert(message);
        };

        console.error = function(message) {
            originalConsole.apply(this, arguments);
            alert(message);
        };
    }

})();


/**
 * cosmetic upgrades
 */
(extendFormatting = () => {

    // Enable Highlight.js
    // Pretty prints code examples
    hljs.initHighlightingOnLoad();


    // Bootstrap does not format tables by default
    document.querySelectorAll('table').forEach(tbl => {
        tbl.classList.add('table');
        tbl.classList.add('table-hover');
    });

    document.querySelectorAll('thead').forEach(thead => {
        thead.classList.add('thead-light');
    });

})();


/**
 * Smooth scroll to top of page
 */
function scrollToTop() {
    const smoothScroll = () => {
        const c = document.documentElement.scrollTop || document.body.scrollTop;

        if (c > 0) {
            window.requestAnimationFrame(scrollToTop);
            window.scrollTo(0, c - c / 8);
        }
    };

    smoothScroll();
}

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

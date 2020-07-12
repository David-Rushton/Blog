/**
 * in non-production environments warnings and errors are made more visible to the user
 */
(extendedLoggingInNonProudctionEnvironments = () => {

    const isProduction = window.location.href.startsWith('https://david-rushton.dev');
    const originalConsole = window.console;

    if( ! isProduction ) {

        console.log('Extended alerting enabled');

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
 * controls upvote behaviour
 */
(enableUpvotes = async () => {

    const id = document.querySelector('head[data-article-id').getAttribute('data-article-id');
    const upvoteTag = document.querySelector('#article-up-vote-count');
    let upvoteScore = 0;

    const makeApiCall = async (method) => await fetch(`/api/v1/upvotes/${id}`, { method: method });
    const updateDisplay = () => upvoteTag.innerText = upvoteScore.toLocaleString();


    // query db for current upvote score
    const articleRecord = await makeApiCall('GET');
    if( ! articleRecord.ok ) {
        console.error(`Cannot retrieve current upvote score.  Id: ${id}`);
        return;
    }

    // read value and display to user
    const articleJson = await articleRecord.json();
    upvoteScore = articleJson.article.upVotes;
    updateDisplay();


    // event handler upvote button
    document.querySelector('#article-up-vote').onclick = async function() {

        console.log(`Upvoting article.  Id: ${id}`);

        // update display first - appears faster to user
        upvoteScore++;
        updateDisplay();

        let response = await makeApiCall('POST');
        if( ! response.ok )
            console.error(`Cannot upvote article.  Id: ${id}`);
    };
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

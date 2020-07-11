// Highlight.js used to highlight syntax
hljs.initHighlightingOnLoad();


// Bootstrap does not format tables by default
// These class have to be applied
document.querySelectorAll('table').forEach(tbl => {
    tbl.classList.add('table');
    tbl.classList.add('table-hover');
});

document.querySelectorAll('thead').forEach(thead => {
    thead.classList.add('thead-light');
});


// smooth scroll back to the top of the page
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


// up vote an article
function upVoteArticle(articleId) {
    window.alert(`upvoting ${articleId}`);
}






/**
 * Fetches the number of up votes for the article
 */
document.body.onload = async function () {


    console.log("updating up dots...");


    const htmlTag = document.getElementsByTagName("head")[0];

    if( ! htmlTag.hasAttribute("data-article-id") )
        return;


    const id = htmlTag.getAttribute("data-article-id");
    const response = await fetch(`/api/v1/upvotes/${id}`, { method: 'GET' });

    if(! response.ok)
        console.error(`cannot retrieve article up-vote count: ${id}`);


    let upVoteTag = document.getElementById('article-up-vote-count');
    let responseJson = await response.json();
    let currentCount = responseJson.article.upVotes;
    upVoteTag.setAttribute('data-up-vote-count', currentCount);
    upVoteTag.innerText = currentCount.toLocaleString();

    console.log("\tcomplete...");
}


/**
 * Handles up votes
 */
document.getElementById("article-up-vote").onclick = async function() {

    console.log("up dotting article...");


    const htmlTag = document.getElementsByTagName("head")[0];

    if( ! htmlTag.hasAttribute("data-article-id") )
        return;


    const id = htmlTag.getAttribute("data-article-id");
    const response = await fetch(`/api/v1/upvotes/${id}`, { method: 'POST' });

    if(! response.ok)
        console.error(`cannot update article up-vote count: ${id}`);


    let upVoteTag = document.getElementById('article-up-vote-count');
    let currentCount = parseInt(upVoteTag.getAttribute('data-up-vote-count')) + 1;
    upVoteTag.setAttribute('data-up-vote-count', currentCount);
    upVoteTag.innerText = currentCount.toLocaleString();


    console.log("\tcomplete...");
}

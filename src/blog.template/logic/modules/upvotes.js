import * as article from './article.js';

const _upvoteTag = document.querySelector('#article-up-vote-count');
let _score = 1;


/**
 * fetches the current upvote score form the db and refreshes the display
 */
(async () => {
    try {

        const articleDoc = await article.getArticle();
        _score = articleDoc.article.upVotes;
    }
    catch(e) {

        throw new upvoteException(e.message || 'cannot download upvote score');
    }
    finally {

        refreshUpVoteScore(_score);
    }
})();


/**
 * updates the displayed upvote score
 * @param {int} score - score to display
 */
function refreshUpVoteScore(score) {

    if( ! _upvoteTag )
        throw new upvoteException('cannot find upvote element');

    _upvoteTag.innerText = score.toLocaleString();
}


/**
 * increments the upvote score
 */
async function incrementUpvoteScore() {

    refreshUpVoteScore(_score++);

    const id = article.getArticleId();
    const response = await fetch(`/api/v1/upvotes/${id}`, { method: method });

    if( ! response.ok ) {
        const responseJson = await response.json();
        throw new upvoteException(responseJson.message || `cannot upvote article: ${id}`);
    }
};


/**
 * returns a custom exception
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function upvoteException(message) {
    this.message = message;

    this.toString = () => message;
}


export { incrementUpvoteScore };

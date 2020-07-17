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
    document.querySelector('#article-up-vote').addEventListener('click', async function() {

        // update display first - appears faster to user
        upvoteScore++;
        updateDisplay();

        let response = await makeApiCall('POST');
        if( ! response.ok )
          console.error(`Cannot upvote article.  Id: ${id}`);
    });
})();

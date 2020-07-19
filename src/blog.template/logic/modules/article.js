let _articleId = null;
let _articleDocument = null;


/**
 * returns the article document form the blog-db
 */
async function getArticle() {
    try {
        // We only need to fetch data once
        if( ! _articleDocument ) {
            const id = getId();
            const response = await fetch(`/api/v1/articles/${id}`, { method: 'GET' });

            if( ! response.ok )
                throw new articleException(`cannot find article id in db: ${id}}`);

            _articleDocument = await response.json();

            console.debug('called article api');
        }

        return _articleDocument
    }
    catch(e) {
        throw new articleException(e.message);
    }
}


/**
 * returns the guid for the current article
 */
function getId() {
    try {
        if( ! _articleId ) {
            // site generator injects the blog-db unique id into the article header
            _articleId = document.querySelector('head[data-article-id').getAttribute('data-article-id');
        }

        return _articleId;
    }
    catch {
        throw new articleException('article id not found');
    }
}


/**
 * returns a custom exception
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function articleException(message) {
    this.message = message;

    this.toString = () => message;
}


export { getId, getArticle };

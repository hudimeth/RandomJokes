import React from 'react';

const ViewAllJoke = ({ joke }) => {
    const { setup, punchline, userJokeLikes } = joke;

    return (
        <div className='container mt-5 col-md-8 offset-md-2 rounded shadow' style={{ backgroundColor: 'whitesmoke' }}>
            <div className='row'>
                <h4 style={{ color: 'blue' }}>{setup}</h4>
                <h5 style={{ color: 'orange' }}>{punchline}</h5>
                <h4>Likes: {userJokeLikes.filter(l => l.liked).length}</h4>
                <h4>Dislikes: {userJokeLikes.filter(l => l.liked === false).length}</h4>
            </div>
        </div>
    )
}
export default ViewAllJoke;
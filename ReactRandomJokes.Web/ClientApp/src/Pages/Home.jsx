import React, { useEffect, useState } from 'react'
import {useAuth } from '../Components/AuthContextComponent'
import axios from 'axios';
import { Link } from 'react-router-dom';

const Home = () => {

    const { user } = useAuth();
    const [joke, setJoke] = useState({
        id: 0,
        originalId: 0,
        setup: '',
        punchline: ''
    });
    const [jokeLikesCount, setJokeLikesCount] = useState({ likes: 0, dislikes: 0 });
    const [interactionStatus, setInteractionStatus] = useState('');
    //const [timeToChangeMind, setTimeToChangeMind] = useState(0);

    const canChangeMind = interactionStatus === 'Liked' || interactionStatus === 'Disliked' || interactionStatus === 'NeverInteracted';

    useEffect(() => {
        loadJoke();
    }, []);

    const loadJoke = async () => {
        const { data } = await axios.get('/api/jokes/randomjoke');
        setJoke(data);
        await loadInteractionStatus(data.id);
    }

    const loadInteractionStatus = async (jokeId) => {
        const { data } = await axios.get(`/api/jokes/getinteractionstatus?jokeid=${jokeId}`);
        console.log(data.status)
        setInteractionStatus(data.status);
    }

    const loadLikesCount = async () => {
        const { data } = await axios.get(`/api/jokes/getlikescount?id=${joke.id}`);
        if (!data) {
            return;
        }
        setJokeLikesCount(data);
    }

    setInterval(loadLikesCount, 500)

    const onRefreshClick = () => window.location.reload();

    const onButtonClick = async like => {
        let likeStatus = false;
        if (like == 'true') {
            likeStatus = true;
        }
        const { data } = await axios.get('/api/account/getcurrentuser');
        await axios.post('/api/jokes/addupdatelikeforjoke', { userId: data.id, jokeId: joke.id, liked: likeStatus });
        await loadInteractionStatus(joke.id);
    }

    const { setup, punchline } = joke;
    if (!interactionStatus) {
        return (
            <div className='container pt-5'>
                <div className="text-center">
                    <div className="spinner-border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                    <h4>Loading an AWESOME joke...</h4>
                </div>
            </div>
        )
    }

    return (<div>
        <div className='container pt-5'>
            <div className='row' style={{ alignItems: 'center', display: 'flex' }}>
                <div className='col-md-6 offset-md-3 bg light p-4 rounded shadow'>
                    <h2>{setup}</h2>
                    <h3 style={{color:'red'} }>{punchline}</h3>
                    {!user && <p><Link to='/login'>Sign in to like this joke</Link></p>}
                    <div>
                        {user && <button
                            disabled={interactionStatus === 'Liked' || interactionStatus == 'CanNoLongerInteract'}
                            className='btn btn-primary'
                            onClick={() => onButtonClick('true')}>Like</button>}
                        {user && <button
                            disabled={interactionStatus === 'Disliked' || interactionStatus == 'CanNoLongerInteract'}
                            className='btn btn-danger mx-2'
                            onClick={onButtonClick}>Dislike</button>}
                    </div>
                    <h3 className='mt-2'>Likes: {jokeLikesCount.likes}</h3>
                    <h3 className='mt-2'>Dislikes: {jokeLikesCount.dislikes}</h3>
                    {canChangeMind && <h6>You have 5 minutes from when you like/dislike the joke to change your mind</h6> }
                </div>
                <div>
                    <button onClick={onRefreshClick} className='mt-5 btn btn-lg btn-info offset-md-3' >This joke isn't enough for you?! Click here to see the next joke.</button>
                </div>
            </div>
        </div>
    </div>
    )
}
export default Home;
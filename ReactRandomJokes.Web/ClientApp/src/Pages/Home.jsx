import React, { useEffect, useState } from 'react'
import { useAuth } from '../Components/AuthContextComponent';
import axios from 'axios';
import { Link } from 'react-router-dom';

const Home = () => {

    //the joke is rendering a second later than the app opens, so the jokelikes aren't able to be shown yet cuz the joke.id is null- need to figure out how to make the loadUserJokeLike to
    //render only once the joke is loaded...

    const { user } = useAuth();
    const [joke, setJoke] = useState(null);
    const [jokeLikesCount, setJokeLikesCount] = useState({ likes: 0, dislikes: 0 });
    const [userJokeLike, setUserJokeLike] = useState(null);

    const loadJoke = async () => {
        const { data } = await axios.get('/api/jokes/randomjoke');
        setJoke(data);
        console.log(`joke: ${data}`)
    }

    const loadUserJokeLike = async () => {
        const { data } = await axios.get(`/api/jokes/jokelikes/${joke.id}`);
        console.log(`joke likes: ${data}`);
        const userLike = data.find((like) => like.userId === user.id);
        console.log(`user like: ${userLike}`);
        if (userLike) {
            setUserJokeLike(userLike.liked);
        }
    }

    //const loadUserJokeLike = async () => {
    //    const { data } = await axios.get('/api/account/getcurrentuser');
    //    if (!data) {
    //        console.log('no user')
    //        return;
    //    }
    //    console.log(data);
    //    const { data: jokeLike } = await axios.get(`/api/jokes/getjokelikeforuser?userId=2&jokeId=${joke.id}`);//user id is null/undefined- keep getting axios error
    //    if (!jokeLike) {
    //        return;
    //    }
    //    setUserJokeLike(jokeLike.liked);
    //    console.log(userJokeLike);
    //}

    useEffect(() => {
        const doStuff = async () => {
            await loadJoke();
            //await loadUserJokeLike();
        }
        doStuff();
    }, []);

    const loadLikesCount = async () => {
        const { data } = await axios.get(`/api/jokes/getlikescount?id=${joke.id}`);
        if (!data) {
            return;
        }
        setJokeLikesCount(data);
    }

    if (joke) {
        setInterval(loadLikesCount, 500)
        loadUserJokeLike();
    }

    const onRefreshClick = () => window.location.reload();

    const onButtonClick = async like => {
        let likeStatus = false;
        if (like == 'true') {
            likeStatus = true;
        }
        const { data } = await axios.get('/api/account/getcurrentuser');
        await axios.post('/api/jokes/addupdatelikeforjoke', { userId: data.id, jokeId: joke.id, liked: likeStatus });
        await loadJoke();
    }

    if (!joke) {
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
        {user && <h1>Welcome Back!</h1>}
        <div className='container pt-5'>
            <div className='row' style={{ alignItems: 'center', display: 'flex' }}>
                <div className='col-md-6 offset-md-3 bg light p-4 rounded shadow'>
                    <h3>{joke.setup}</h3>
                    <h3>{joke.punchline}</h3>
                    {!user && <p><Link to='/login'>Sign in to like this joke</Link></p>}
                    <div>
                        {user && <button disabled={userJokeLike} className='btn btn-primary' onClick={() => onButtonClick('true')}>Like</button>}
                        {user && <button disabled={userJokeLike === false} className='btn btn-danger mx-2' onClick={onButtonClick}>Dislike</button>}
                    </div>
                    <h3 className='mt-2'>Likes: {jokeLikesCount.likes}</h3>
                    <h3 className='mt-2'>Dislikes: {jokeLikesCount.dislikes}</h3>
                    <h3>userJokeLike: <button className='btn btn-info' onClick={loadUserJokeLike}>get jokelikes for joke</button></h3>
                    <button className='btn btn-info' onClick={console.log(joke)}>see joke</button>
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
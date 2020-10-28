import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import Todo from './components/Todo/Todo'
import './custom.css'
import { Color } from './components/Color';
import SnakeApp from './components/Snake/SnakeApp';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/counter' component={Counter} />
                <Route path='/fetch-data' component={FetchData} />
                <Route path="/color" component={Color} />
                <Route path="/todo" component={Todo} />
                <Route path="/snake" component={SnakeApp} />
            </Layout>
        );
    }
}

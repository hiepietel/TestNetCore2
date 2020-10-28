import React, { Component } from 'react';
import Header from './layout/Header'
import Todos from './Todos';
import AddTodo from './AddTodo'
import './App.css';

export default class Todo extends Component {

    state = {
        todos: [
            {
                id: 1,
                title: 'clean the windows',
                completed: false
            },
            {
                id: 2,
                title: 'have no sleep for more than 24 hours',
                completed: false
            },
            {
                id: 3,
                title: 'refill petrol in the car',
                completed: false
            }
        ]
    }

    markComplete = (id) => {
        //  console.log('From App.js');
        //  console.log(id);
        this.setState({
            todos: this.state.todos.map(todo => {
                if (todo.id === id) {
                    todo.completed = !todo.completed;
                }
                return todo;
            })
        })

    }
    delTodo = (id) => {
        console.log('del: ' + id);
        this.setState({ todos: [...this.state.todos.filter(todo => todo.Id !== id)] });
    }
    render() {
        //console.log(this.state.todos)
        return (
            <div className="row">   
                <div className="column">
                    <Header />
                    <AddTodo />
                    <h1>
                        <Todos todos={this.state.todos}
                            markComplete={this.markComplete}
                            delTodo={this.delTodo} />
                    </h1>
                </div>
                
                
            </div>

            //<div className="App">
            //    <div className='container' style = {this.conStyle}>
            //        <div style={this.appStyle}>
            //            <Header />
            //            <AddTodo />
            //            <h1>
            //                <Todos todos={this.state.todos}
            //                    markComplete={this.markComplete}
            //                    delTodo={this.delTodo} />
            //            </h1>
            //        </div>
            //        <div style={this.appStyle}>
            //            <TestApi />
            //        </div>

            //    </div>

            //</div>
        );
    }

}
const conStyle = {
    maxWidth : "1000px"
}
const appStyle = {
    width: "100%",
    padding: "1px",
    float: "left"
}


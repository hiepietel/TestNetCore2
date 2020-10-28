import React, { Component } from 'react'
import PropTypes from 'prop-types';

export class TodoItem extends Component {
    getStyle = () => {
        return {
            textDecoration : this.props.todo.completed ? 'line-through' : 'none',
            backgroundColor : 'f4f4f4',
            padding : '10px',
            borderBottom: '1px #ccc dotted'
        }
        // if(this.props.todo.completed){
        //     return {
        //         textDecoration: 'line-through'
        //     }
        // }
        // else{
        //     return {textDecoration: 'none'}
        // }
    }
    
    markComplete = (e) => {
        
    }

    render() {
        const {id, title } = this.props.todo;
        return (
            <div style={this.getStyle()}>
                <p>
                    <input type='checkbox' onChange={this.props.markComplete.bind(this, id)}/> {' '}
                    { title }
                    <button onClick = {this.props.delTodo.bind(this, id)} style= {btnStyle}>x</button>
                </p> 
            </div>
        )
    }
}

TodoItem.propTypes = {
    todo: PropTypes.object.isRequired
}
const btnStyle = {
    background : 'red',
    color: '#fff',
    border: 'none',
    padding: '5px 10px',
    borderRadius: '50%',
    cursor: 'pointer',
    float: 'right'

}
//const itemStyle = {
//    backgroundColor: "grey",
//    }
export default TodoItem

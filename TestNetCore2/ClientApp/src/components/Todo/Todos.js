import React, {Component} from 'react';
import TodoItem  from './TodoItem'
import PropTypes from 'prop-types';
class Todos extends Component{



render() { 
  //console.log(this.state.todos)
  return this.props.todos.map((todo) => (
  //<h3>{ todo.title }</h3>
  <TodoItem key = {todo.key} todo = {todo} 
  markComplete = {this.props.markComplete} 
  delTodo={this.props.delTodo}/>
  ));
}
}
Todos.propTypes = {
    todos: PropTypes.array.isRequired
}

export default Todos;
//rce -> compoment
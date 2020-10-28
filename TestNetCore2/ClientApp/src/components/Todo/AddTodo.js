import React, { Component } from 'react'

export class AddComponent extends Component {
    render() {
        return (
            <form style={{display: 'flex'}}>
                <input type='text' 
                    name='title'
                    style={{flex: '10', padding: '5px'}} 
                    placeholder='Add todo...'></input>
                <input type='submit' 
                    value='Submit'
                    className='btn'
                    style={{flex: '1'}}>
                    </input> 
            </form>
        )
    }
}

export default AddComponent

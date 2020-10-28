import React from 'react'

function ColorHeader() {
    return (
        <header style={headerStyle}>
            <h1>Color</h1>
        </header>
    )
}

const headerStyle = {
    background: "#8a3",
    color: '#ccf',
    textAlign: 'center',
    padding: '10px'
}
export default ColorHeader;

import React from 'react'

function CovidHeader() {
    return (
        <header style={headerStyle}>
            <h1>Covid</h1>
        </header>
    )
}

const headerStyle = {
    background: "#abc",
    color: '#050',
    textAlign: 'center',
    padding: '10px'
}
export default CovidHeader;

import React, { Component } from 'react';
import { SwatchesPicker , HuePicker } from 'react-color';
import axios from 'axios';
export class Color extends Component {
    static displayName = Color.name;

    constructor(props) {
        super(props);
        this.state = {
            allColors: [],
            loading: true,
            buttonColor: '#fff',
            color : null
        };
    };
    componentDidMount() {
        //this.getAllColors();
    };
    PostColor = (color) => {
        console.log(color)
        const colorToSet = { Red: color.rgb.r, Green: color.rgb.g, Blue: color.rgb.b, Brightness: 255 };
        const response = axios.post('Color/2', colorToSet);
        axios.post('Color/3', colorToSet);
    };
    static renderAllColors(allColors) {
        return (
            <div>
                {allColors.map(allColor =>
                    // <button style={{ backgroundColor: { allColor. } }}> {allColor.name}</button> 
                    <button style={{ ...buttonStyle, ...{ backgroundColor: allColor.name } }} >&nbsp;</button >
                )}
            </div>
        );

    }
    handleClick() {
        console.log("123")
    }
    handleChangeComplete = (colorr) => {
        this.setState({ buttonColor: colorr.hex });
        this.setState({ color: colorr });
        console.log(colorr);
        this.PostColor(colorr);
    };
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Color.renderAllColors(this.state.allColors);
        return (
            <div>
                <h1>Color</h1>
                <div>
                <SwatchesPicker
                    width="100%"
                    height="480px`"
                    color={this.state.background}
                    onChangeComplete={this.handleChangeComplete}
                    />
                    </div>
                {contents}
            </div>
        );
    }
    async getAllColors() {
        const response = await fetch('color/all');
        const data = await response.json();
        this.setState({ allColors: data, loading: false });
    }
}
const SetColorButtonStyle = {
    width : "100%"
}
const buttonStyle = {
    border: "1px solid #000000",
    width: "10%",
    heigth: "50px"
}

import React, { Component } from 'react';
import axios from 'axios';

class Device extends Component {
    state = {
        devices: [],
        devicesLoaded: false
    }

    componentDidMount() {
        this.getDevices();
    }

    getDevices = () => {
        axios.get(`device/all`).then(
            res => {
                const devices = res.data;
                this.setState({ devices });
                this.setState({ devicesLoaded: true });
            })
    }
    renderDeviceTable = () => {
        return (
            <div>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <tr>
                    <th>Name</th>
                    <th>Ip</th>
                        <th>Description</th>
                        </tr>
                    {this.state.devices.map(dev =>
                        <tr>
                            <td>{dev.name}</td>
                            <td>{dev.ip}</td>
                            <td>{dev.description}</td>
                            </tr>
                    )}
                </table>
            </div>
        );
    }

    render() {
        let devicesTable = this.state.devicesLoaded
            ? this.renderDeviceTable()
            : <p><em>Loading...</em></p>

        return (
            <div>
                <h1> device</h1>
                {devicesTable}
            </div>
        )
    }
}
export default Device
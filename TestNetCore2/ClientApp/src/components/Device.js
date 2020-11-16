import React, { Component, useEffect, useState } from 'react';
import axios from 'axios';
//https://app.pluralsight.com/guides/dynamic-tables-from-editable-columns-in-react-html
class Device extends Component {
    state = {
        devices: [],
        devicesLoaded: false,
        inEditMode: {
            status: false,
            rowKey: null
        },
        setInEditMode: {
            status: false,
            rowKey: null
        },
        unitPrice: null,
        setUnitPrice: null
    };

    componentDidMount() {
        this.getDevices();
    };

    onEdit = (id, currentUnitPrice) => {
        this.setState({
            inEditMode: {
                status: true,
                rowKey: id
            }
        });
        this.setState({ setUnitPrice: currentUnitPrice });
    };
onSave = ({ id, newUnitPrice }) => {
    console.log(id);
    //updateInventory({ id, newUnitPrice });
}

onCancel = () => {
    // reset the inEditMode state value
    this.setState({
        setInEditMode: {
            status: false,
            rowKey: null
        }
    });
    // reset the unit price state value
    this.setState({ setUnitPrice: null });
}



getDevices = () => {
    axios.get(`device/all`).then(
        res => {
            const devices = res.data;
            this.setState({ devices });

            this.setState({ devicesLoaded: true });
        })
};
renderDeviceTable = () => {
    return (
        <div>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Ip</th>
                        <th>Description</th>
                        <th>Function</th>
                        <th>IsAlive</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.devices.map(dev =>
                        <tr>
                            <td>{dev.name}</td>
                            <td>{
                                this.state.inEditMode.status && this.state.inEditMode.rowKey === dev.id ? (
                                    <input value={this.state.unitPrice}
                                        onChange={(event) => this.setState({ setUnitPrice: event.target.value })}
                                    />
                                ) : (
                                        dev.ip
                                    )
                            }
                            </td>
                            <td>
                                {
                                    this.state.inEditMode.status && this.state.inEditMode.rowKey === dev.id ? (
                                        <React.Fragment>
                                            <button
                                                className={"btn-success"}
                                                onClick={() => this.onSave(dev.id, dev.ip)}
                                            >
                                                Save
                                            </button>

                                            <button
                                                className={"btn-secondary"}
                                                style={{ marginLeft: 8 }}
                                                onClick={() => this.onCancel()}
                                            >
                                                Cancel
                                            </button>
                                        </React.Fragment>
                                    ) : (
                                            <button
                                                className={"btn-primary"}
                                                onClick={() => this.onEdit(dev.id, dev.ip)}
                                            >
                                                Edit
                                            </button>
                                        )
                                }
                            </td>
                            <td>{dev.description}</td>
                            <td>{dev.function}</td>
                            <td>{dev.isAlive === true ? "YES" : "NO"}</td>
                        </tr>
                    )}
                </tbody>
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
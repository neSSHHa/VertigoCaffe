import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div className="container">
                {/*<NavMenu />*/}
                
                <div >
                    <div></div>
                    <div>
                        <Container tag="main">
                            {this.props.children}
                        </Container>
                    </div>
                    <div></div>
                </div>
            </div>
        );
    }
}

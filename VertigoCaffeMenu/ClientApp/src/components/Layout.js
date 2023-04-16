import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import './itemsandcategories.css';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        {/*<NavMenu />*/}
            <Container tag="main">
                <div className={"container"}>
                    <div className={"row"}>
                        <div className={"col-lg-3 col-md-2 col-sm-12 col-xs-12"}>

                        </div>
                        <div className={"col-lg-6 col-md-8 col-sm-12 col-xs-12 wrapper"}>
                            <div className={"info"}>
                                <div className={"mainInfo"}>
                                    <h1>Vertigo</h1>
                                    <h4>Caffe bar</h4>
                                <p id="num">060 4142027</p>
                                <p id="adress">Momira Gajica 15A, Velika Plana</p>
                            </div>
                            <div className={"timeInfo"}>
                                <p>Ponedeljak: 7:00-22:00</p>
                                <p>Utorak: 7:00-22:00</p>
                                <p>Sreda: 7:00-22:00</p>
                                <p>Cetvrtak: 7:00-22:00</p>
                                <p>Petak: 7:00-22:00</p>
                                <p>Subota: 7:00-22:00</p>
                                <p>Nedelja: Zatvoreno</p>
                                </div>
                            </div>
                            <div className={"mt-5"}>
                            {this.props.children}
                            </div>
                            </div>
                        <div className={"col-lg-3 col-md-2 col-sm-4 col-xs-12"}>

                        </div>
                    </div>
                </div>
         
        </Container>
      </div>
    );
  }
}

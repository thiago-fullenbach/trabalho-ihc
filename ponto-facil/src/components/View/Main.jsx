import React from "react";
import Header from '../Generic/Header';
import Menu from '../Generic/Menu';
import Content from '../Generic/Content';
import Footer from '../Generic/Footer';
import './Main.css';
import Logo from "../Generic/Logo";

const Main = () => {
    return (
        <div className="Main">
            <Logo></Logo>
            <Header></Header>
            <Menu></Menu>
            <Content></Content>
            <Footer></Footer>
        </div>
    )
}

export default Main;
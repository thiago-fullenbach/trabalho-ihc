import React from "react";
import Header from '../Generic/Header/Header';
import Menu from '../Generic/Menu/Menu';
import Content from '../Generic/Content/Content';
import Footer from '../Generic/Footer/Footer';
import './Main.css';
import Logo from "../Generic/Logo/Logo";

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
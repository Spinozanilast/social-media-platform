import React from "react";
import Logo from "./Logo";
import SearchBar from "./SearchBar";

export default function Navbar() {
    return (
        <header className=" mx-auto flex flex-row justify-center h-8 items-center mt-page-part">
            <div className="logo-container absolute mt-page-part ml-page-part">
                <Logo />
            </div>
            <SearchBar placeholderText="Search For ..." />
            <div>{/* TODO: Some feature in future */}</div>
        </header>
    );
}

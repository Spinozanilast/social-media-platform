import React from 'react';
import Logo from './Logo';
import SearchBar from './SearchBar';

export default function Navbar() {
    return (
        <div className="mx-auto flex flex-row justify-center h-8 items-center m-page-part">
            <div className="logo-container absolute mt-page-part ml-page-part h-fit">
                <Logo animationType="forward" className="text-left" />
            </div>
            <SearchBar placeholderText="Search For ..." />
        </div>
    );
}

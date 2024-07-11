import React from "react";
import { MdOutlineSearch } from "react-icons/md";

type SearchBarProps = {
    placeholderText: string;
};

export default function SearchBar({ placeholderText }: SearchBarProps) {
    return (
        <div className="h-full">
            <input
                type="text"
                placeholder={placeholderText}
                className="search-bar bg-background-secondary placeholder:text-center min-w-96 h-full text-white border-2 text-center focus:border-accent-orange"
            />
            <button className="rounded-button bg-background-inline-button">
                <MdOutlineSearch color="white" className="m-auto" />
            </button>
        </div>
    );
}

import React from "react";
import { SearchOutlined } from "@mui/icons-material";

type SearchBarProps = {
    placeholderText: string;
    className?: string;
};

export default function SearchBar({
    placeholderText,
    className,
}: SearchBarProps) {
    return (
        <div className={className}>
            <input
                type="text"
                placeholder={placeholderText}
                className="search-bar bg-background-secondary placeholder:text-center min-w-96 h-full text-white border-2 text-center focus:border-accent-orange"
            />
            <button className="rounded-button bg-background-inline-button">
                <SearchOutlined className="m-auto" />
            </button>
        </div>
    );
}

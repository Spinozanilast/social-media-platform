import React from 'react';
import { FaSearch } from 'react-icons/fa';

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
            <button className="rounded-button flex justify-center items-center min-h-[36px] min-w-[36px] bg-background-inline-button hover:bg-accent-orange group">
                <FaSearch className="h-full text-white relative self-center transition-transform duration-150 group-active:scale-90" />
            </button>
        </div>
    );
}

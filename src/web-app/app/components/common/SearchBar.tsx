'use client';
import React from 'react';
import { FaSearch } from 'react-icons/fa';
import UrlCompleteItemData from '../../data/CompleteItemsData/UrlCompleteItemData';
import {
    Autocomplete,
    AutocompleteItem,
    AutocompleteSection,
} from '@nextui-org/react';
import UrlCompletionItem from './CompleteItems/UrlCompletionItem';
import { SiCurl } from 'react-icons/si';
import createUrlCompletionItem from './CompleteItems/UrlCompletionItem';

type SearchBarProps = {
    placeholderText: string;
    urlCompletions: UrlCompleteItemData[];
};

export default function SearchBar({
    placeholderText,
    urlCompletions,
}: SearchBarProps) {
    const headingClasses =
        'flex w-full sticky top-1 z-20 py-1.5 px-2 bg-default-100 shadow-small rounded-small';

    return (
        <Autocomplete
            aria-label={placeholderText}
            classNames={{
                base: 'max-w-xs',
                listboxWrapper: 'max-h-[320px]',
                selectorButton: 'text-default-500',
            }}
            inputProps={{
                classNames: {
                    input: 'ml-1',
                    inputWrapper: 'h-[48px]',
                },
            }}
            listboxProps={{
                hideSelectedIcon: true,
                itemClasses: {
                    base: [
                        'rounded-medium',
                        'text-default-500',
                        'transition-opacity',
                        'data-[hover=true]:text-foreground',
                        'dark:data-[hover=true]:bg-default-50',
                        'data-[pressed=true]:opacity-70',
                        'data-[hover=true]:bg-default-200',
                        'data-[selectable=true]:focus:bg-default-100',
                        'data-[focus-visible=true]:ring-default-500',
                    ],
                },
            }}
            placeholder={placeholderText}
            popoverProps={{
                offset: 10,
                classNames: {
                    base: 'rounded-large',
                    content:
                        'p-1 border-small border-default-100 bg-background',
                },
            }}
            radius="full"
            startContent={
                <FaSearch
                    className="text-default-400"
                    size={20}
                    strokeWidth={2.5}
                />
            }
            variant="bordered"
        >
            <AutocompleteSection
                classNames={{
                    heading: headingClasses,
                }}
                showDivider
                title="Urls"
            >
                {urlCompletions.map((item) => createUrlCompletionItem(item))}
            </AutocompleteSection>
        </Autocomplete>
    );
}

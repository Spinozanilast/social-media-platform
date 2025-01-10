'use client';
import React from 'react';
import { FaAngleDown } from 'react-icons/fa';
import CompleteItemData from '../../data/CompleteItemData';
import { Select, SelectSection } from '@nextui-org/react';
import createUrlCompletionItem from './CompleteItems/UrlCompletionItem';

type SearchBarProps = {
    placeholderText: string;
    urlCompletions: CompleteItemData[];
};

export default function SearchBar({
    placeholderText,
    urlCompletions,
}: SearchBarProps) {
    const headingClasses =
        'flex w-full sticky z-20 py-1.5 px-2 bg-default-100 shadow-small rounded-small';

    return (
        <Select
            aria-label={placeholderText}
            classNames={{
                base: 'min-w-[200px]',
                listboxWrapper: 'max-h-[320px]',
            }}
            listboxProps={{
                hideSelectedIcon: true,
                itemClasses: {
                    base: [
                        'rounded-medium',
                        'text-default-900',
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
                    base: 'rounded-md',
                    content: 'p-1 border-small border-default-100',
                },
            }}
            radius="lg"
            startContent={
                <FaAngleDown
                    className="text-default-400"
                    size={20}
                    strokeWidth={2}
                />
            }
            variant="bordered"
        >
            <SelectSection
                classNames={{
                    heading: headingClasses,
                }}
                showDivider
                title="Urls"
            >
                {urlCompletions.map((item) => createUrlCompletionItem(item))}
            </SelectSection>
        </Select>
    );
}

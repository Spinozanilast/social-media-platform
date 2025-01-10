'use client';
import type { Selection } from '@nextui-org/react';
import React, { use, useEffect, useState } from 'react';
import { Locale, locales } from '@/i18n/i18n.config';
import {
    SelectItem,
    Dropdown,
    DropdownTrigger,
    Button,
    DropdownMenu,
    Badge,
} from '@nextui-org/react';
import { useLocale, useTranslations } from 'next-intl';
import { IoLanguage } from 'react-icons/io5';
import { setUserLocale } from '@/app/services/locale';

const LangSwitcherTranslationSection = 'LangSwitcher';

export type LangSwitchSelectProps = {
    badgeOnLeft?: boolean;
};

const LangSwitchSelect = ({ badgeOnLeft = true }: LangSwitchSelectProps) => {
    const t = useTranslations(LangSwitcherTranslationSection);
    const locale = useLocale();
    const existingLocales = locales;
    const [isOpen, setIsOpen] = useState(false);

    const handleButtonClick = () => {
        setIsOpen(!isOpen);
    };

    const setCurrentLocale = (locale: any) => {
        setIsOpen(false);
        setUserLocale(locale[0] as Locale);
    };

    const [selectedKeys, setSelectedKeys] = useState<Selection>(
        new Set([locale])
    );

    useEffect(() => {
        setCurrentLocale(Array.from(selectedKeys));
    }, [selectedKeys]);

    return (
        <Dropdown isOpen={isOpen}>
            <DropdownTrigger>
                <Badge
                    content={locale.slice(0, 2)}
                    placement={badgeOnLeft ? 'bottom-left' : 'bottom-right'}
                    size="sm"
                    className="share-tech-mono"
                    variant="faded"
                    showOutline={false}
                >
                    <Button
                        className="utility-small-icon min-w-fit"
                        onClick={handleButtonClick}
                    >
                        <IoLanguage />
                    </Button>
                </Badge>
            </DropdownTrigger>
            <DropdownMenu
                disallowEmptySelection
                selectionMode="single"
                selectedKeys={selectedKeys}
                onSelectionChange={setSelectedKeys}
            >
                {existingLocales.map((locale) => (
                    <SelectItem key={locale} onClick={() => setIsOpen(false)}>
                        {t(locale)}
                    </SelectItem>
                ))}
            </DropdownMenu>
        </Dropdown>
    );
};

export default LangSwitchSelect;

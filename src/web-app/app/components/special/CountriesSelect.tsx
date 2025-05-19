'use client';

import { Select, SelectItem, Avatar } from '@heroui/react';
import React, { useState, useEffect } from 'react';
import { getCountries } from '@api/profile/service';
import { Country } from '@api/profile/types';

interface CountrySelectProps {
    value: Country | undefined;
    onChange: (country: Country) => void;
}

const CountrySelect = ({ value, onChange }: CountrySelectProps) => {
    const [countries, setCountries] = useState<Country[]>([]);

    useEffect(() => {
        getCountries().then((c) => setCountries(c));
    }, []);

    const handleCountryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedCountryId = e.target.value;

        const selectedCountry = countries.find(
            (country) => country.id.toString() === selectedCountryId
        );
        if (selectedCountry) {
            onChange(selectedCountry);
        }
    };

    return (
        <Select
            label="Country"
            listboxProps={{
                itemClasses: {
                    base: [
                        'rounded-md',
                        'text-default-500',
                        'transition-opacity',
                        'data-[hover=true]:text-foreground',
                        'data-[hover=true]:bg-default-100',
                        'dark:data-[hover=true]:bg-default-50',
                        'data-[selectable=true]:focus:bg-default-50',
                        'data-[pressed=true]:opacity-70',
                        'data-[focus-visible=true]:ring-default-500',
                    ],
                },
            }}
            classNames={{
                label: 'group-data-[filled=true]:-translate-y-5',
                trigger: 'min-h-16',
                listboxWrapper: 'max-h-[400px]',
            }}
            selectedKeys={[value?.id.toString()!]}
            onChange={handleCountryChange}
            variant="flat"
            items={countries}
            renderValue={(items) => {
                return items.map((c) => (
                    <div key={c.key} className="flex items-center gap-1 m-1">
                        <Avatar
                            alt={c.data?.isoCode}
                            className="w-6 h-6"
                            src={`https://flagcdn.com/${c.data?.isoCode.toLowerCase()}.svg`}
                        />
                        <div className="flex flex-col">
                            <span>{c.data?.name}</span>
                            <span className="text-default-500 text-tiny">
                                ({c.data?.isoCode})
                            </span>
                        </div>
                    </div>
                ));
            }}
        >
            {(country) => (
                <SelectItem
                    key={country.id}
                    endContent={country.isoCode}
                    startContent={
                        <Avatar
                            alt={country.isoCode}
                            className="w-6 h-6"
                            src={`https://flagcdn.com/${country.isoCode.toLowerCase()}.svg`}
                        />
                    }
                >
                    {country.name}
                </SelectItem>
            )}
        </Select>
    );
};

export default CountrySelect;

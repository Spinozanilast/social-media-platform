'use client';
import ProfileService from '@/app/api/services/profile';
import Country from '@/app/models/Profiles/country';
import { Select, SelectItem, Avatar } from '@nextui-org/react';
import { useState, useEffect } from 'react';

interface CountrySelectProps {
    value: Country | undefined;
    onChange: (country: Country) => void;
}

const CountrySelect = ({ value, onChange }: CountrySelectProps) => {
    const [countries, setCountries] = useState<Country[]>([]);

    const fetchCountries = async () => {
        const countries = await ProfileService.getCountries();
        setCountries(countries);
    };

    useEffect(() => {
        fetchCountries();
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
            selectedKeys={[value?.id.toString()!]}
            onChange={handleCountryChange}
            variant="flat"
        >
            {countries.map((country) => (
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
            ))}
        </Select>
    );
};

export default CountrySelect;

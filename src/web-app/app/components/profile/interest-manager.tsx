'use client';
import { Button, Input } from '@heroui/react';
import { useState } from 'react';

type InterestManagerProps = {
    interests: string[];
    onUpdate: (interests: string[]) => void;
};

const InterestManager = ({ interests, onUpdate }: InterestManagerProps) => {
    const [newInterest, setNewInterest] = useState('');

    const addInterest = () => {
        if (newInterest.trim().length >= 2) {
            onUpdate([...interests, newInterest.trim()]);
            setNewInterest('');
        }
    };

    return (
        <div className='space-y-2'>
            <h3 className="font-semibold">Interests</h3>
            <div className="flex gap-2 mb-1 w-full">
                {interests.map((interest, index) => (
                    <div key={index} className="relative group flex flex-row gap-2 items-center">
                        <Input
                            value={interest}
                            size="sm"
                            onChange={(e) => {
                                const updated = [...interests];
                                updated[index] = e.target.value;
                                onUpdate(updated);
                            }}
                        />
                        <Button
                            size="sm"
                            className="absolute right-0 opacity-0 group-hover:opacity-100"
                            color="danger"
                            isIconOnly
                            onPress={() =>
                                onUpdate(
                                    interests.filter((_, i) => i !== index)
                                )
                            }
                        >
                            ×
                        </Button>
                    </div>
                ))}
            </div>
            <div className="flex gap-2">
                <Input
                    value={newInterest}
                    onChange={(e) => setNewInterest(e.target.value)}
                    placeholder="Add new interest"
                />
                <Button onPress={addInterest}>Add</Button>
            </div>
        </div>
    );
};

export default InterestManager;

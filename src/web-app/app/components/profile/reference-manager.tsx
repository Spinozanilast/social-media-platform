'use client';
import { Button, Input } from '@heroui/react';
import { useState } from 'react';

type ReferenceManagerProps = {
    references: string[];
    onUpdate: (references: string[]) => void;
};

export default function ReferenceManager({
    references,
    onUpdate,
}: ReferenceManagerProps) {
    const [newRef, setNewRef] = useState('');

    const addReference = () => {
        if (newRef.trim()) {
            onUpdate([...references, newRef.trim()]);
            setNewRef('');
        }
    };

    return (
        <div className="space-y-2">
            <h3 className="font-semibold">References</h3>
            <div className="space-y-2">
                {references.map((ref, index) => (
                    <div key={index} className="flex gap-2 items-center">
                        <Input
                            value={ref}
                            onChange={(e) => {
                                const updated = [...references];
                                updated[index] = e.target.value;
                                onUpdate(updated);
                            }}
                            fullWidth
                        />
                        <Button
                            size="sm"
                            color="danger"
                            isIconOnly
                            onPress={() =>
                                onUpdate(
                                    references.filter((_, i) => i !== index)
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
                    value={newRef}
                    onChange={(e) => setNewRef(e.target.value)}
                    placeholder="Add new reference"
                />
                <Button onPress={addReference}>Add</Button>
            </div>
        </div>
    );
}

import React, { useState } from 'react';
import {
    Button,
    DatePicker,
    Divider,
    Input,
    Modal,
    ModalBody,
    ModalContent,
    ModalFooter,
    ModalHeader,
    Spinner,
} from '@heroui/react';
import { parseDate } from '@internationalized/date';
import ImageTooltip from '../common/ImageTooltip';
import { Profile } from '@api/profile/types';
import CountrySelect from '@components/special/CountriesSelect';
import InterestManager from '@components/profile/InterestManager';
import ReferenceManager from '@components/profile/ReferencesManager';
import Image from 'next/image';

interface EditProfileProps {
    profileInfo: Profile;
    imageUrl: string;
    isOpen: boolean;
    onOpenChange: () => void;
    onSave: (profile: Profile) => Promise<void>;
    onImageUpload: (image: Blob) => Promise<void>;
}

const EditProfile: React.FC<EditProfileProps> = ({
    profileInfo,
    imageUrl,
    isOpen,
    onOpenChange,
    onSave,
    onImageUpload,
}) => {
    const [form, setform] = useState<Profile>(profileInfo);
    const [selectedImage, setSelectedImage] = useState<File | null>(null);
    const [loading, setLoading] = useState(false);

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) setSelectedImage(file);
    };

    const handleSubmit = async () => {
        setLoading(true);
        try {
            if (selectedImage) await onImageUpload(selectedImage);
            await onSave(form);
            onOpenChange();
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal
            backdrop="blur"
            isOpen={isOpen}
            className="max-h-fit overflow-y-auto min-w-[400px]"
            onOpenChange={onOpenChange}
        >
            <ModalContent>
                <ModalHeader className="border-b">Edit Profile</ModalHeader>

                <ModalBody className="space-y-4">
                    <div className="space-y-4">
                        <div className="flex items-center justify-center gap-4">
                            <ImageTooltip
                                imageUrl={
                                    selectedImage
                                        ? URL.createObjectURL(selectedImage)
                                        : imageUrl
                                }
                                toolTipProps={{
                                    content: 'Preview new profile image',
                                }}
                                width={300}
                            >
                                <div className="relative group flex justify-center">
                                    <Image
                                        src={
                                            selectedImage
                                                ? URL.createObjectURL(
                                                      selectedImage
                                                  )
                                                : imageUrl
                                        }
                                        alt={'tooltip image'}
                                        width={64}
                                        height={64}
                                        className="object-cover"
                                    />
                                    <div
                                        className="rounded-sm absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100
                                            transition-opacity"
                                    >
                                        <input
                                            type="file"
                                            accept="image/*"
                                            onChange={handleImageChange}
                                            className="absolute inset-0 opacity-0 cursor-pointer"
                                        />
                                    </div>
                                </div>
                            </ImageTooltip>
                        </div>

                        <Divider />

                        <Input
                            label="About"
                            name="about"
                            value={form.about}
                            onChange={(e) =>
                                setform({ ...form, about: e.target.value })
                            }
                            maxLength={100}
                        />

                        <div className="grid md:grid-cols-2 gap-4">
                            <CountrySelect
                                value={form.country}
                                onChange={(country) =>
                                    setform({ ...form, country })
                                }
                            />
                            <DatePicker
                                classNames={{
                                    input: 'min-h-16',
                                }}
                                showMonthAndYearPickers
                                label="Birth Date"
                                value={
                                    form.birthDate
                                        ? parseDate(
                                              form.birthDate.split('T')[0]
                                          )
                                        : null
                                }
                                onChange={(date) => {
                                    const isoDate = new Date(
                                        Date.UTC(
                                            date!.year,
                                            date!.month - 1,
                                            date!.day
                                        )
                                    ).toISOString();

                                    setform({ ...form, birthDate: isoDate });
                                }}
                            />
                        </div>

                        <Divider />

                        <InterestManager
                            interests={form.interests || []}
                            onUpdate={(interests: string[]) =>
                                setform({ ...form, interests })
                            }
                        />

                        <ReferenceManager
                            references={form.references || []}
                            onUpdate={(references: string[]) =>
                                setform({ ...form, references })
                            }
                        />
                    </div>
                </ModalBody>

                <ModalFooter className="border-t justify-between">
                    <Button variant="flat" onPress={onOpenChange}>
                        Cancel
                    </Button>
                    <Button
                        color="primary"
                        onPress={handleSubmit}
                        isDisabled={loading}
                    >
                        {loading ? <Spinner size="sm" /> : 'Save Changes'}
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

export default EditProfile;

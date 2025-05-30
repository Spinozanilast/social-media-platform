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
import { getLocalTimeZone, parseDate, today } from '@internationalized/date';
import ImageTooltip from '~components/common/image-tooltip';
import { Profile } from '~api/profile/types';
import CountrySelect from '~/components/common/countries-select';
import InterestManager from '~/components/profile/interest-manager';
import ReferenceManager from '~/components/profile/reference-manager';
import Image from 'next/image';

interface EditProfileProps {
    profileInfo: Profile;
    imageUrl: string;
    isOpen: boolean;
    onOpenChange: () => void;
    onProfileInfoSave: (profile: Profile) => Promise<void>;
    onImageUpload: (image: Blob) => Promise<void>;
}

const EditProfileModal: React.FC<EditProfileProps> = ({
    profileInfo,
    imageUrl,
    isOpen,
    onProfileInfoSave,
    onOpenChange,
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
            await onProfileInfoSave(form);
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

                <ModalBody className="space-y-4 mb-4">
                    <div className="space-y-4">
                        <div className="flex items-center justify-center gap-4">
                            <ImageTooltip
                                imageUrl={
                                    selectedImage
                                        ? URL.createObjectURL(selectedImage)
                                        : imageUrl
                                }
                                toolTipProps={selectedImage ? {
                                    content: 'Preview new profile image',
                                } : undefined}
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
                                    base: "h-full",
                                    inputWrapper: 'h-full',
                                }}
                                showMonthAndYearPickers
                                label="Birth Date"
                                errorMessage={(value) => {
                                    if (value.isInvalid) {
                                        return "Please enter a valid date.";
                                    }
                                }}
                                maxValue={today(getLocalTimeZone())}
                                value={
                                    form.birthDate
                                        ? parseDate(
                                            form.birthDate.split('T')[0]
                                        )
                                        : null
                                }
                                onChange={(date) => {
                                    if (!date) return;
                                    const formattedDate = `${date.year}-${String(date.month).padStart(2, '0')}-${String(date.day).padStart(2, '0')}`;
                                    setform({ ...form, birthDate: formattedDate });
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

export default EditProfileModal;

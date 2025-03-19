import React, { useState } from 'react';
import {
    Modal,
    Input,
    Button,
    ModalHeader,
    ModalBody,
    ModalFooter,
    ModalContent,
    Divider,
    DatePicker,
    DateValue,
} from '@heroui/react';
import {
    CalendarDate,
    getLocalTimeZone,
    parseDate,
    today,
} from '@internationalized/date';
import ImageTooltip from '../common/ImageTooltip';
import CountrySelect from '../special/CountriesSelector';
import Profile from '../../api/types/profiles/profile';

interface EditProfileProps {
    profileInfo: Profile;
    image: Blob | null;
    isOpen: boolean;
    onOpenChange: () => void;
    onSave: (updatedProfile: Profile) => void;
    onImageUpload: (image: Blob) => void;
}

const MIN_INTEREST_LENGTH = 2;
const MIN_REFERENCE_LENGTH = 0;

const EditProfile: React.FC<EditProfileProps> = ({
    profileInfo,
    image,
    isOpen,
    onOpenChange,
    onSave,
    onImageUpload,
}) => {
    const defaultFormState: Profile = {
        ...profileInfo,
        birthDate: profileInfo.birthDate
            ? new Date(profileInfo.birthDate)
            : undefined,
    };

    const [form, setForm] = useState<Profile>(defaultFormState);

    const [newInterest, setNewInterest] = useState('');
    const [newRef, setNewRef] = useState('');

    const [selectedImage, setSelectedImage] = useState<Blob | null>(null);
    const [selectedImageUrl, setSelectedImageUrl] = useState<string>(
        image ? URL.createObjectURL(image) : '/profile.svg'
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setForm((prevForm) => ({ ...prevForm, [name]: value }));
    };

    const handleDateChange = (date: CalendarDate | null) => {
        setForm((prevForm) => ({
            ...prevForm,
            birthDate: date ? date.toString() : undefined,
        }));
    };

    const handleAddInterest = () => {
        if (newInterest.length > MIN_INTEREST_LENGTH) {
            setForm((prevForm) => ({
                ...prevForm,
                interests: [...prevForm.interests, newInterest],
            }));
            setNewInterest('');
        }
    };

    const handleAddRef = () => {
        if (newRef.length > MIN_REFERENCE_LENGTH) {
            setForm((prevForm) => ({
                ...prevForm,
                references: [...prevForm.references, newRef],
            }));
            setNewRef('');
        }
    };

    const handleEditInterest = (index: number, value: string) => {
        const updatedInterests = [...form.interests];
        updatedInterests[index] = value;
        setForm((prevForm) => ({ ...prevForm, interests: updatedInterests }));
    };

    const handleDeleteInterest = (index: number) => {
        const updatedInterests = form.interests.filter((_, i) => i !== index);
        setForm((prevForm) => ({ ...prevForm, interests: updatedInterests }));
    };

    const handleEditRef = (index: number, value: string) => {
        const updatedreferences = [...form.references];
        updatedreferences[index] = value;
        setForm((prevForm) => ({ ...prevForm, references: updatedreferences }));
    };

    const handleDeleteRef = (index: number) => {
        const updatedreferences = form.references.filter((_, i) => i !== index);
        setForm((prevForm) => ({ ...prevForm, references: updatedreferences }));
    };

    const handleSaveProfile = () => {
        if (selectedImage) {
            onImageUpload(selectedImage);
            setSelectedImageUrl(URL.createObjectURL(selectedImage));
        }
        onSave(form);
        onOpenChange();
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0] || null;
        setSelectedImage(file);
    };

    return (
        <Modal
            backdrop="blur"
            isOpen={isOpen}
            className="max-h-full overflow-y-auto min-w-[400px]"
            placement="center"
            onOpenChange={onOpenChange}
        >
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalHeader>Edit Profile</ModalHeader>
                        <ModalBody>
                            <div className="flex flex-col gap-2">
                                <div className="flex flex-col gap-2">
                                    <h3>Profile Image</h3>
                                    <div className="flex flex-row gap-1 items-center">
                                        <Input
                                            className="h-full"
                                            placeholder="Upload Image"
                                            color="primary"
                                            type="file"
                                            accept="image/*"
                                            onChange={handleImageChange}
                                        />
                                        {selectedImage && (
                                            <ImageTooltip
                                                toolTipProps={{
                                                    placement: 'bottom',
                                                }}
                                                width={200}
                                                imageUrl={selectedImageUrl}
                                                imageProps={{ isZoomed: true }}
                                            >
                                                <Button
                                                    size="md"
                                                    className="cursor-pointer"
                                                    color="primary"
                                                    onPress={handleSaveProfile}
                                                >
                                                    Upload Image
                                                </Button>
                                            </ImageTooltip>
                                        )}
                                    </div>
                                </div>
                                <h3>About yourself</h3>
                                <Input
                                    label="About"
                                    name="about"
                                    maxLength={100}
                                    value={form.about}
                                    onChange={handleChange}
                                />
                                <Input
                                    label="Anything"
                                    name="anything"
                                    maxLength={200}
                                    value={form.anything || ''}
                                    onChange={handleChange}
                                />
                                <CountrySelect
                                    value={form.country}
                                    onChange={(country) =>
                                        setForm((prevForm) => ({
                                            ...prevForm,
                                            country,
                                        }))
                                    }
                                />
                                <DatePicker
                                    showMonthAndYearPickers
                                    label="Birth Date"
                                    value={
                                        form.birthDate
                                            ? parseDate(
                                                  form.birthDate
                                                      ? new Date(form.birthDate)
                                                            .toISOString()
                                                            .split('T')[0]
                                                      : ''
                                              )
                                            : today(getLocalTimeZone())
                                    }
                                    onChange={(date) => handleDateChange(date)}
                                />
                                <Divider />
                                <div className="flex flex-col gap-2">
                                    <h3>Interests</h3>
                                    {form.interests.map((interest, index) => (
                                        <div
                                            key={index}
                                            className="flex items-center gap-2"
                                        >
                                            <Input
                                                size="sm"
                                                value={interest}
                                                onChange={(e) =>
                                                    handleEditInterest(
                                                        index,
                                                        e.target.value
                                                    )
                                                }
                                            />
                                            <Button
                                                size="sm"
                                                color="danger"
                                                variant="flat"
                                                onPress={() =>
                                                    handleDeleteInterest(index)
                                                }
                                            >
                                                Delete
                                            </Button>
                                        </div>
                                    ))}
                                    <Input
                                        size="md"
                                        label="Add Interest"
                                        value={newInterest}
                                        onChange={(e) =>
                                            setNewInterest(e.target.value)
                                        }
                                    />
                                    <Button
                                        size="md"
                                        onPress={handleAddInterest}
                                    >
                                        Add
                                    </Button>
                                </div>
                                <Divider />
                                <div className="flex flex-col gap-2">
                                    <h3>References</h3>
                                    {form.references.map((ref, index) => (
                                        <div
                                            key={index}
                                            className="flex items-center gap-2"
                                        >
                                            <Input
                                                size="sm"
                                                value={ref}
                                                onChange={(e) =>
                                                    handleEditRef(
                                                        index,
                                                        e.target.value
                                                    )
                                                }
                                            />
                                            <Button
                                                size="sm"
                                                color="danger"
                                                variant="flat"
                                                onPress={() =>
                                                    handleDeleteRef(index)
                                                }
                                            >
                                                Delete
                                            </Button>
                                        </div>
                                    ))}
                                    <Input
                                        size="md"
                                        label="Add Reference"
                                        value={newRef}
                                        onChange={(e) =>
                                            setNewRef(e.target.value)
                                        }
                                    />
                                    <Button size="md" onPress={handleAddRef}>
                                        Add
                                    </Button>
                                </div>
                            </div>
                        </ModalBody>
                        <ModalFooter>
                            <Button
                                color="danger"
                                variant="flat"
                                onPress={onClose}
                            >
                                Cancel
                            </Button>
                            <Button color="primary" onPress={handleSaveProfile}>
                                Save
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    );
};

export default EditProfile;

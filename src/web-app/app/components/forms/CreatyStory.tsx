'use client';
import React, { useState } from 'react';
import {
    Button,
    Input,
    Card,
    CardBody,
    CardHeader,
    Divider,
    Spinner,
} from '@nextui-org/react';
import StoriesService from '@api/services/story';
import { CreateStoryModel } from '@/app/models/Stories/stories';
import { ValidationResult } from '@/app/models/validation-result';
import MarkdownIt from 'markdown-it';
import MdEditor from 'react-markdown-editor-lite';
import 'react-markdown-editor-lite/lib/index.css';

const mdParser = new MarkdownIt();

interface CreateStoryProps {
    authorId: string;
    onClose: () => void;
}

const CreateStory: React.FC<CreateStoryProps> = ({ onClose, authorId }) => {
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [tags, setTags] = useState('');
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<ValidationResult | null>(null);

    const handleEditorChange = ({
        html,
        text,
    }: {
        html: string;
        text: string;
    }) => {
        setContent(text);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        const createStoryModel: CreateStoryModel = {
            title,
            content,
            tags: tags.split(',').map((tag) => tag.trim()),
            authorId: authorId,
            isShared: false,
        };
        try {
            const result = await StoriesService.createStory(createStoryModel);
            if (result.isValid) {
                setTitle('');
                setContent('');
                setTags('');
            }
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
            onClose();
        }
    };

    return (
        <Card className="w-full h-full">
            <CardHeader className="flex flex-col gap-2">
                <h1 className="text-xl text-accent-orange font-bold">
                    Create a New Story
                </h1>
            </CardHeader>
            <Divider />
            <CardBody className="w-full h-full flex flex-col">
                <form
                    onSubmit={handleSubmit}
                    className="flex flex-col gap-4 min-h-full"
                >
                    <Input
                        placeholder="Story Title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                    <MdEditor
                        value={content}
                        style={{ height: '500px' }}
                        renderHTML={(text) => mdParser.render(text)}
                        onChange={handleEditorChange}
                    />
                    <Input
                        placeholder="Tags (comma separated)"
                        value={tags}
                        onChange={(e) => setTags(e.target.value)}
                    />
                    <Button
                        className="w-min self-center"
                        variant="shadow"
                        color="primary"
                        type="submit"
                        disabled={loading}
                    >
                        {loading ? <Spinner /> : 'Create Story'}
                    </Button>
                </form>
                {message && (
                    <div
                        className={`mt-4 ${
                            message.isValid ? 'text-success' : 'text-danger'
                        }`}
                    >
                        {message.errors.map((error) => (
                            <p key={error.propertyName}>{error.errorMessage}</p>
                        ))}
                    </div>
                )}
            </CardBody>
        </Card>
    );
};

export default CreateStory;

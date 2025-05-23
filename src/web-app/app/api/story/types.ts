export interface Story {
    id: number;
    authorId: string;
    title: string;
    content: string;
    tags: string[];
    createdAt: Date;
    updatedAt: Date;
    originalPostId?: number;
    isShared: boolean;
    isEdited: boolean;
    originalPost?: Story;
}

export interface CreateStoryModel {
    authorId: string;
    title: string;
    content: string;
    tags: string[];
    isShared?: boolean;
}

export interface UpdateStoryModel {
    title?: string;
    content?: string;
    tags?: string[];
    isShared?: boolean;
    originalPostId?: number;
}

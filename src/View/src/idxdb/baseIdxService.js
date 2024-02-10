import { openDB } from 'idb';
import { DB_NAME } from "@/idxdb/constants.js";

export async function initializeDB(STORE_NAME) {
    return await openDB(DB_NAME, 1, {
        upgrade(db) {
            if (!db.objectStoreNames.contains(STORE_NAME)) {
                db.createObjectStore(STORE_NAME, { keyPath: 'id', autoIncrement: true });
            }
        }
    });
}
# DevFolio: Project Tags Feature

This document contains the **complete code and documentation** for the Project Tagging system in DevFolio.  
It demonstrates a **full-stack enhancement**: relational database design, service-layer logic, and frontend integration.

---

## Database Schema

The tagging system requires two new tables:

```sql
CREATE TABLE IF NOT EXISTS tags (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS project_tags (
  project_id INT NOT NULL,
  tag_id INT NOT NULL,
  PRIMARY KEY (project_id, tag_id),
  FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE,
  FOREIGN KEY (tag_id) REFERENCES tags(id) ON DELETE CASCADE
);
```

This defines a **many-to-many relationship** between `projects` and `tags`.

---

## Backend Code


```js
// server/src/services/projectTags.js
// Extended service layer for DevFolio: add tagging & filtering to projects.
// Demonstrates relational queries, validation, and multi-table joins.

import pool from "../db.js";

/**
 * Attach one or more tags to a project.
 * Creates tags if they don't exist, then links them to the project.
 */
export async function addTagsToProject(projectId, tags = []) {
  if (!Array.isArray(tags) || tags.length === 0) return [];

  const conn = await pool.getConnection();
  try {
    await conn.beginTransaction();

    // Insert tags if they don't exist
    for (const tag of tags) {
      await conn.execute(
        "INSERT IGNORE INTO tags (name) VALUES (?)",
        [tag.toLowerCase()]
      );
    }

    // Link tags to project
    for (const tag of tags) {
      await conn.execute(
        `INSERT IGNORE INTO project_tags (project_id, tag_id)
         SELECT ?, id FROM tags WHERE name = ?`,
        [projectId, tag.toLowerCase()]
      );
    }

    await conn.commit();
    return getTagsForProject(projectId);
  } catch (err) {
    await conn.rollback();
    throw err;
  } finally {
    conn.release();
  }
}

/**
 * Get all tags linked to a project.
 */
export async function getTagsForProject(projectId) {
  const [rows] = await pool.query(
    `SELECT t.name
     FROM tags t
     JOIN project_tags pt ON pt.tag_id = t.id
     WHERE pt.project_id = ?`,
    [projectId]
  );
  return rows.map(r => r.name);
}

/**
 * Get all projects for a user filtered by tag name.
 */
export async function getProjectsByTag(userId, tag) {
  const [rows] = await pool.query(
    `SELECT p.*
     FROM projects p
     JOIN project_tags pt ON pt.project_id = p.id
     JOIN tags t ON t.id = pt.tag_id
     WHERE p.user_id = ? AND t.name = ?
     ORDER BY p.created_at DESC`,
    [userId, tag.toLowerCase()]
  );
  return rows;
}
```

---

## Frontend Code


```jsx
// client/src/ui/ProjectTags.jsx
// Tag management UI for DevFolio projects.
// Shows full-stack fluency: controlled inputs, fetch calls, async state mgmt.

import { useEffect, useState } from "react";

export default function ProjectTags({ projectId, token }) {
  const [tags, setTags] = useState([]);
  const [newTag, setNewTag] = useState("");
  const [loading, setLoading] = useState(false);

  // Load tags for this project
  useEffect(() => {
    async function fetchTags() {
      const res = await fetch(`http://localhost:4000/api/projects/${projectId}/tags`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      const data = await res.json();
      setTags(data || []);
    }
    fetchTags();
  }, [projectId, token]);

  // Add a tag to this project
  async function handleAddTag(e) {
    e.preventDefault();
    if (!newTag.trim()) return;

    setLoading(true);
    const res = await fetch(`http://localhost:4000/api/projects/${projectId}/tags`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ tags: [newTag] }),
    });

    const data = await res.json();
    setTags(data); // updated list from API
    setNewTag("");
    setLoading(false);
  }

  return (
    <div className="border p-3 rounded bg-gray-50">
      <h4 className="font-bold mb-2">Tags</h4>
      <div className="flex flex-wrap gap-2 mb-3">
        {tags.map((t) => (
          <span
            key={t}
            className="px-2 py-1 text-sm bg-blue-200 rounded-full"
          >
            {t}
          </span>
        ))}
        {tags.length === 0 && <span className="text-gray-500">No tags yet</span>}
      </div>

      <form onSubmit={handleAddTag} className="flex gap-2">
        <input
          className="border px-2 py-1 flex-1 rounded"
          placeholder="Add a tag (e.g. React)"
          value={newTag}
          onChange={(e) => setNewTag(e.target.value)}
        />
        <button
          type="submit"
          className="bg-blue-600 text-white px-3 py-1 rounded disabled:opacity-50"
          disabled={loading}
        >
          {loading ? "Adding..." : "Add"}
        </button>
      </form>
    </div>
  );
}
```

---

## Example Flow

1. User visits a project → `GET /api/projects/:id/tags` retrieves tags  
2. User types "React" and clicks Add → `POST /api/projects/:id/tags` creates/links tag  
3. Backend inserts the tag if missing and links it to the project  
4. UI re-renders with updated tag list  
5. Projects can be filtered by tag using `getProjectsByTag`  



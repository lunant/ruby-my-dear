using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[SelectionBase]
public class CircleRenderer : ShapeRenderer {

    // max polygon side length
    const float MAX_FRAGMENT_LENGTH = 0.03f;
    // if diameter changes by more than this recreate mesh
    const float INNER_MESH_RETAIN_THRESHOLD = 0.3f;

    public CircleProperty property = new CircleProperty();
    CircleProperty cachedProperty = new CircleProperty();
    float innerMeshDiameter; // updated with mesh

    // WARN: Assign to child GameObjects in prefab
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;

    /* ShapeRenderer */

    protected void UpdateGameObject() {
        center = property.center;
        diameter = property.diameter;
    }

    protected bool UpdateMeshIfNeeded() {
        if ((Mathf.Abs(property.diameter - innerMeshDiameter) / innerMeshDiameter) >
                INNER_MESH_RETAIN_THRESHOLD) {
            // mesh dimension change
            UpdateInnerMesh();
            UpdateBorderMesh();
        } else if (property.border.MeshNeedsUpdate(cachedProperty.border)) {
            // border property change
            UpdateBorderMesh();
        }

        // color change
        if (property.color != cachedProperty.color) {
            UpdateInnerMeshColor(property.color);
        }

        if (property.border.style != BorderStyle.None &&
            property.border.color != cachedProperty.border.color) {
            UpdateBorderMeshColor(property.border.color);
        }

    }

    protected bool GameObjectWasModified() {
        // TODO
        return false;
    }

    protected ShapeProperty GameObjectToShapeProperty() {
        // TODO
        return null;
    }

    /*
     * Create Mesh
     */

    void UpdateInnerMesh() {
        CreateInner();
        innerMeshDiameter = property.diameter;
    }

    void UpdateBorderMesh() {
        switch (property.border.style) {
            case BorderStyle.Solid:
                CreateBorderSolid();
                break;
            case BorderStyle.Dash:
                // TODO
                CreateBorderSolid();
                break;
            case BorderStyle.None:
                RemoveBorder();
                break;
        }
    }

    void UpdateInnerMeshColor(Color color) {
        MeshUtil.UpdateColor(innerMeshRenderer, color);
    }

    void UpdateBorderMeshColor(Color color) {
        MeshUtil.UpdateColor(borderMeshRenderer, color);
    }

    /* RENDERING */

    void RemoveBorder() {
        using (var vh = new VertexHelper()) {
            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
    }

    void CreateInner() {

        using (var vh = new VertexHelper()) {

            int numTris = Mathf.CeilToInt(diameter * Mathf.PI / MAX_FRAGMENT_LENGTH);
            float centerAngle = 2*Mathf.PI/numTris;
            Color c = Color.white;

            // create verticies
            vh.AddVert(Vector3.zero, c, Vector2.zero); // midpoint
            for (int i = 0; i < numTris; i++) {
                float angle = centerAngle * i;
                float x = Mathf.Cos(angle) * 0.5f;
                float y = Mathf.Sin(angle) * 0.5f;
                vh.AddVert(new Vector3(x, y, 0), c, Vector2.zero);
            }

            // create triangles
            for (int i = 0; i < numTris-1; i++) {
                vh.AddTriangle(0, i+2, i+1);
            }
            // come around
            vh.AddTriangle(0, 1, numTris);
            MeshUtil.UpdateMesh(innerMeshFilter, vh);
        }
    }

    void CreateBorderSolid() {

        using (var vh = new VertexHelper()) {

            int numQuads = Mathf.CeilToInt(diameter * Mathf.PI / MAX_FRAGMENT_LENGTH);
            float centerAngle = 2*Mathf.PI/numQuads;
            float scaledBorderThickness = property.border.thickness/property.diameter;
            Color c = Color.white;

            for (int i = 0; i<numQuads; i++) {
                float angle = centerAngle * i;
                float x = Mathf.Cos(angle) * 0.5f;
                float y = Mathf.Sin(angle) * 0.5f;

                float scaledInnerRadius = 1; // default = outer

                switch (property.border.position) {
                    case BorderPosition.Center:
                        scaledInnerRadius -= scaledBorderThickness/2;
                        break;

                    case BorderPosition.Inside:
                        scaledInnerRadius -= scaledBorderThickness;
                        break;
                }

                float scaledOuterRadius = scaledInnerRadius + scaledBorderThickness;

                vh.AddVert(new Vector3(x*scaledInnerRadius, y*scaledInnerRadius, 0),
                           c, Vector2.zero);

                vh.AddVert(new Vector3(x*scaledOuterRadius, y*scaledOuterRadius, 0),
                           c, Vector2.zero);

            }

            for (int i = 0; i<numQuads-1; i++) {
                int idxBase = 2*i; // two new vertices per quad
                vh.AddTriangle(idxBase, idxBase+2, idxBase+1);
                vh.AddTriangle(idxBase+1, idxBase+2, idxBase+3);
            }

            int finalIdxBase = 2*numQuads-2; // last two vertices
            vh.AddTriangle(finalIdxBase, 0, finalIdxBase+1);
            vh.AddTriangle(finalIdxBase+1, 0, 1);

            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
    }

    void CreateBorderDash() {

    }

    /*
     * Getter/Setters
     */

    public Vector2 center {
        get { return transform.position; }

        set { transform.position = new Vector3(value.x, value.y, transform.position.z); }
    }

    public float diameter {
        get { return transform.localScale.x; }

        set { transform.localScale = new Vector3(value, value, 1); }
    }
}


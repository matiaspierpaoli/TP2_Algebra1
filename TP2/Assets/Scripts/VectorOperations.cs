using UnityEngine;
using System;

public class VectorOperations : MonoBehaviour
{
    public Vector3 initialVector; // Vector inicial proporcionado por el usuario
    private Vector3 vectorA; // Primer vector (igual al inicial)
    private Vector3 vectorB; // Segundo vector, rotado 90º respecto al primero
    private Vector3 vectorC; // Tercer vector, perpendicular a los otros dos

    // Puntos de corte
    private Vector3 point1; 
    private Vector3 point2; // Segundo punto de corte
    private Vector3 point3; // Tercer punto de corte

    void Start()
    {
        InitializeVectors(initialVector); // Inicializar los vectores a partir del vector inicial
        SliceVectors(); // Realizar el corte de los vectores
        CalculateArea(); // Calcular el área de las caras de la pirámide
    }

    // Inicializa los vectores A, B y C a partir del vector inicial
    void InitializeVectors(Vector3 initial)
    {
        vectorA = initial; // Asignar el vector inicial a vectorA

        // Calcular vector rotado 90º respecto al primero
        vectorB = RotateVector90(vectorA);

        // Calcular vector perpendicular a los otros dos
        vectorC = CrossProduct(vectorA, vectorB);

        Debug.Log($"Vector A: {vectorA}");
        Debug.Log($"Vector B: {vectorB}");
        Debug.Log($"Vector C: {vectorC}");
    }

    // Calcula un vector rotado 90º respecto al vector dado
    Vector3 RotateVector90(Vector3 vector)
    {
        return new Vector3(-vector.y, vector.x, vector.z);
    }

    // Calcula el producto cruz de dos vectores
    Vector3 CrossProduct(Vector3 v1, Vector3 v2)
    {
        // El producto cruz de dos vectores en 3D resulta en un vector perpendicular a ambos
        // "https://es.khanacademy.org/math/multivariable-calculus/thinking-about-multivariable-function/x786f2022:vectors-and-matrices/a/cross-products-mvc"
        return new Vector3(
            v1.y * v2.z - v1.z * v2.y,
            v1.z * v2.x - v1.x * v2.z,
            v1.x * v2.y - v1.y * v2.x
        );
    }

    // Realiza el corte de los vectores para obtener los puntos de la base de la pirámide
    void SliceVectors()
    {
        if (vectorA.y < vectorB.y)
        {
            point1 = vectorA; // Asigna vectorA como el punto más bajo
            point2 = CalculateSlicePoint(vectorB, point1.y); // Calcula el punto de corte de vectorB
            point3 = CalculateSlicePoint(vectorC, point1.y); // Calcula el punto de corte de vectorC
        }
        else
        {
            point1 = vectorB; // Asigna vectorB como el punto más bajo
            point2 = CalculateSlicePoint(vectorA, point1.y); // Calcula el punto de corte de vectorA
            point3 = CalculateSlicePoint(vectorC, point1.y); // Calcula el punto de corte de vectorC
        }

        Debug.Log($"Point 1: {point1}");
        Debug.Log($"Point 2: {point2}");
        Debug.Log($"Point 3: {point3}");
    }

    // Calcula el punto de corte de un vector a una altura determinada
    Vector3 CalculateSlicePoint(Vector3 vector, float sliceHeight)
    {
        float factor = sliceHeight / vector.y; // Factor de escala para la altura de corte
        return new Vector3(vector.x * factor, sliceHeight, vector.z * factor); // Punto escalado a la altura de corte
    }

    // Calcula el área de las tres caras de la pirámide y la suma de sus áreas
    void CalculateArea()
    {
        // Calcula el área de los triángulos formados por los puntos y el origen (0,0,0)
        float area1 = CalculateTriangleArea(point1, point2, Vector3.zero);
        float area2 = CalculateTriangleArea(point1, point3, Vector3.zero);
        float area3 = CalculateTriangleArea(point2, point3, Vector3.zero);

        // Mostrar las áreas de los triángulos y la suma de las áreas en consola
        Debug.Log($"Area of Triangle 1: {area1}");
        Debug.Log($"Area of Triangle 2: {area2}");
        Debug.Log($"Area of Triangle 3: {area3}");
        Debug.Log($"Sum of the pyramid's 3 faces: {area1 + area2 + area3}");
    }

    // Calcula el área de un triángulo dado sus tres vértices
    float CalculateTriangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Calcula las distancias entre los puntos del triángulo
        float a = Vector3Distance(p1, p2); // Distancia entre p1 y p2
        float b = Vector3Distance(p2, p3); // Distancia entre p2 y p3
        float c = Vector3Distance(p3, p1); // Distancia entre p3 y p1

        // Fórmula de Herón para calcular el área de un triángulo -> "https://es.wikipedia.org/wiki/F%C3%B3rmula_de_Her%C3%B3n"
        float s = (a + b + c) / 2; // Semiperímetro del triángulo
        return Mathf.Sqrt(s * (s - a) * (s - b) * (s - c)); // Área del triángulo
    }

    // Calcula la distancia entre dos puntos en 3D
    float Vector3Distance(Vector3 p1, Vector3 p2)
    {
        // Calcula la distancia euclidiana entre dos puntos usando el teorema de Pitágoras
        return Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2) + Mathf.Pow(p2.z - p1.z, 2));
    }

    void OnDrawGizmos()
    {
        if (vectorA != Vector3.zero && vectorB != Vector3.zero && vectorC != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, vectorA); // Dibuja vectorA
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, vectorB); // Dibuja vectorB
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, vectorC); // Dibuja vectorC

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point1, 0.1f); // Dibuja punto1
            Gizmos.DrawSphere(point2, 0.1f); // Dibuja punto2
            Gizmos.DrawSphere(point3, 0.1f); // Dibuja punto3
        }
    }
}

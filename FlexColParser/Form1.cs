using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace FlexColParser
{
    public partial class Form1 : Form
    {
        private string objFileName;
        
        public class MaterialNameConverter : TypeConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new[] { "Undefined", "Character", "Water", "InkWater", "Ice"
                    , "Dirt", "Soil", "Wasteland", "Grass", "Cloth", "Carpet", "Rubber", "Vinyl", "Plastic"
                    , "Glass", "Wood", "Stone", "Asphalt", "Sand", "Metal", "IronSand", "Fence", "RopeNet", "Snow"
                    , "CoralSand", "KebaInk", "BlowerInhale", "Ink", "Barrier", "Shield", "ThrowingWeapon", "ExPaint"
                    , "LockerObj", "PlayerMammal", "Torpedo", "CoopFloat", "Ball", "BallEx", "BallGoal"});
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void createJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JObject jsonObject = new JObject();

            // Crear la sección de colisión
            JObject collision = new JObject();
            JObject collisionData = new JObject();
            collisionData.Add("obj_path", "/" + objFileName);

            JObject materials = new JObject();

            // Aquí obtenemos los nombres de materiales del ListBox y los agregamos al JSON
            foreach (string materialName in listBox1.Items)
            {
                materials.Add(materialName, CreateMaterial(materialName));
            }
            string userText = textBoxObjectName.Text;
            string userText2 = textBox69.Text;

            collisionData.Add("materials", materials);
            collision.Add($"/Phive/Shape/Dcc/{userText}.Nin_NX_NVN.bphsh", collisionData);
            jsonObject.Add("collision", collision);

            // Crear la sección de parches de mapa
            JObject mapPatches = new JObject();
            JObject mapPatchData = new JObject();
            mapPatchData.Add("extend_col_heap", checkBoxExtendColHeap.Checked);
            mapPatches.Add(userText2, mapPatchData);
            jsonObject.Add("map_patches", mapPatches);

            string jsonOutput = jsonObject.ToString();

            // Guardar JSON en archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json";
            saveFileDialog.Title = "Guardar archivo JSON";
            saveFileDialog.FileName = "output.json"; // Nombre de archivo predeterminado

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, jsonOutput);
                MessageBox.Show("JSON creado y guardado exitosamente.");
            }
            else
            {
                MessageBox.Show("Operación de creación y guardado de JSON cancelada.");
            }
        }

        private JObject CreateMaterial(string matName, List<string> matFlags = null, string fxPreset = null, List<string> colDisableFlag = null)
        {
            JObject material = new JObject();
            if (matName != null)
                material.Add("mat_name", matName);
            if (matFlags != null)
                material.Add("mat_flags", JToken.FromObject(matFlags));
            if (fxPreset != null)
                material.Add("fx_preset", fxPreset);
            if (colDisableFlag != null)
                material.Add("col_disable_flag", JToken.FromObject(colDisableFlag));

            return material;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OBJ Files (*.obj)|*.obj|All Files (*.*)|*.*";
            openFileDialog.Title = "Select an OBJ file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string objFilePath = openFileDialog.FileName;
                objFileName = Path.GetFileName(objFilePath);
                List<string> materialNames = GetMaterialNamesFromObj(objFilePath);

                listBox1.Items.Clear();
                foreach (string materialName in materialNames)
                {
                    listBox1.Items.Add(materialName);
                }
            }
        }

        static List<string> GetMaterialNamesFromObj(string objFilePath)
        {
            List<string> materialNames = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(objFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("usemtl"))
                        {
                            string[] parts = line.Split(' ');
                            if (parts.Length >= 2)
                            {
                                string materialName = parts[1];
                                materialNames.Add(materialName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo leer el archivo .obj: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return materialNames;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                // Mostrar las propiedades del material seleccionado en el PropertyGrid
                string materialName = listBox1.SelectedItem.ToString();
                propertyGrid1.SelectedObject = new Material { MatName = materialName };
            }
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Material
    {
        [TypeConverter(typeof(Form1.MaterialNameConverter))]
        public string MatName { get; set; }

        public List<string> MatFlags { get; set; }
        public string FxPreset { get; set; }
        public List<string> ColDisableFlag { get; set; }

        public Material()
        {
            MatFlags = new List<string>();
            ColDisableFlag = new List<string>();
        }
    }
}

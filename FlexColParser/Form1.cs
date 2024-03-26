using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;

//llevo sin programar tiempo no me toqueis as bolas aoprfajorovr

namespace FlexColParser
{
    public partial class Form1 : Form
    {
        private string objFileName;
        private List<MaterialInfo> materialsList;

        public class MaterialInfo
        {
            public string OriginalName { get; set; }
            public Material Material { get; set; }
        }

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
                return new StandardValuesCollection(new[]
                {
                    "Undefined", "Character", "Water", "InkWater", "Ice", "Dirt", "Soil", "Wasteland", "Grass", "Cloth",
                    "Carpet", "Rubber", "Vinyl", "Plastic", "Glass", "Wood", "Stone", "Asphalt", "Sand", "Metal",
                    "IronSand", "Fence", "RopeNet", "Snow", "CoralSand", "KebaInk", "BlowerInhale", "Ink", "Barrier",
                    "Shield", "ThrowingWeapon", "ExPaint", "LockerObj", "PlayerMammal", "Torpedo", "CoopFloat", "Ball",
                    "BallEx", "BallGoal"
                });
            }
        }

        public class FxPresetConverter : TypeConverter
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
                return new StandardValuesCollection(new[] { null, "Fence", "Water" });
            }
        }

        public Form1()
        {
            InitializeComponent();
            materialsList = new List<MaterialInfo>();
        }

        private void mecagoendios_Click(object sender, EventArgs e)
        {
            JObject jsonObject = new JObject();

            // sección de colisión
            JObject collision = new JObject();
            JObject collisionData = new JObject();
            collisionData.Add("obj_path", "/" + objFileName);

            JObject materials = new JObject();

            // nombres de materiales del ListBox y los agregamos al JSON
            foreach (MaterialInfo materialInfo in materialsList)
            {
                materials.Add(materialInfo.OriginalName,
                    CreateMaterial(materialInfo.Material.MatName, materialInfo.Material.MatFlags,
                        materialInfo.Material.FxPreset, materialInfo.Material.ColDisableFlag));
            }

            string userText = textBoxObjectName.Text;
            string userText2 = textBox69.Text;

            collisionData.Add("materials", materials);
            collision.Add($"Phive/Shape/Dcc/{userText}.Nin_NX_NVN.bphsh", collisionData);
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
            saveFileDialog.Title = "Save JSON";
            saveFileDialog.FileName = "config.json"; // Nombre de archivo predeterminado

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, jsonOutput);
                MessageBox.Show("Created JSON and saved.");
            }
            else
            {
                MessageBox.Show("cancel what!!!??.");
            }
        }

        private JObject CreateMaterial(string matName, List<string> matFlags = null, string fxPreset = null,
            List<string> colDisableFlag = null)
        {
            JObject material = new JObject();
            if (matName != null)
                material.Add("mat_name", matName);
            if (matFlags != null && matFlags.Count > 0) // coño ya
                material.Add("mat_flags", JToken.FromObject(matFlags));
            if (fxPreset != null)
                material.Add("fx_preset", fxPreset);
            if (colDisableFlag != null && colDisableFlag.Count > 0) // saojijsa08gjsr
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
                materialsList.Clear();
                foreach (string materialName in materialNames)
                {
                    listBox1.Items.Add(materialName);
                    materialsList.Add(new MaterialInfo
                        { OriginalName = materialName, Material = new Material { MatName = "Asphalt" } });
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
                MessageBox.Show("Couldn't read the .obj file: " + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return materialNames;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                // Mostrar las propiedades del material seleccionado en el PropertyGrid
                MaterialInfo materialInfo = materialsList[listBox1.SelectedIndex];
                propertyGrid1.SelectedObject = materialInfo.Material;
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid1.Refresh();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void openbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a JSON file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonFilePath = openFileDialog.FileName;
                string jsonContent = File.ReadAllText(jsonFilePath);

                // Parsear el JSON
                JObject jsonObject = JObject.Parse(jsonContent);

                // Limpiar la lista y el control ListBox
                materialsList.Clear();
                listBox1.Items.Clear();

                // Obtener los datos del property grid
                JObject collision = (JObject)jsonObject["collision"];
                JObject collisionData = collision?.Properties().FirstOrDefault()?.Value as JObject;
                string objPath = collisionData?["obj_path"]?.ToString();

                JObject materials = collisionData?["materials"] as JObject;
                if (materials != null)
                {
                    foreach (KeyValuePair<string, JToken> materialData in materials)
                    {
                        string originalName = materialData.Key;
                        JObject materialInfo = materialData.Value as JObject;
                        string matName = materialInfo?["mat_name"]?.ToString();
                        string fxPreset = materialInfo?["fx_preset"]?.ToString();

                        // Obtener el contenido de mat_flags y col_disable_flag
                        List<string> matFlags =
                            materialInfo?["mat_flags"]?.ToObject<List<string>>() ?? new List<string>();
                        List<string> colDisableFlag = materialInfo?["col_disable_flag"]?.ToObject<List<string>>() ??
                                                      new List<string>();

                        // Agregar los nuevos elementos a la lista y al listBox
                        materialsList.Add(new MaterialInfo
                        {
                            OriginalName = originalName,
                            Material = new Material
                            {
                                MatName = matName, FxPreset = fxPreset, MatFlags = matFlags,
                                ColDisableFlag = colDisableFlag
                            }
                        });
                        listBox1.Items.Add(originalName);
                    }
                }

                // Obtener la información de "map_patches" y actualizar el TextBox y CheckBox
                JObject mapPatches = jsonObject["map_patches"] as JObject;
                if (mapPatches != null)
                {
                    var firstPatch = mapPatches.Properties().FirstOrDefault();
                    if (firstPatch != null)
                    {
                        string patchName = firstPatch.Name;
                        textBox69.Text = patchName;

                        // Obtener el valor de "extend_col_heap" dentro del primer elemento de "map_patches"
                        bool extendColHeap = firstPatch.Value["extend_col_heap"]?.ToObject<bool>() ?? false;
                        checkBoxExtendColHeap.Checked = extendColHeap;
                    }
                }

                textBoxObjectName.Text = Path.GetFileNameWithoutExtension(objPath);
            }
        }
        
        

        //me quiero matar
        public class Material
        {
            
            private List<string> _matFlags;
            private List<string> _ColDisableFlag;
            
            [Category("Attributes")]
            [DisplayName("Material Name")]
            [Description("This will change the sounds of the player's footsteps.")]
            [TypeConverter(typeof(Form1.MaterialNameConverter))]
            public string MatName { get; set; }

            [Category("Attributes")]
            [Editor("System.Windows.Forms.Design.StringCollectionEditor, " + "System.Design, Version=2.0.0.0, " +
                    "Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
            public List<string> MatFlags 
            {
                get { return _matFlags; }
                set
                {
                    _matFlags = value;
                    UpdateFlagsProperties();
                }
            }
            
            private void UpdateFlagValue(string flag, bool value)
            {
                if (value)
                {
                    // Agregar la string si no está presente
                    if (_matFlags == null)
                        _matFlags = new List<string>();
                    if (!_matFlags.Contains(flag))
                        _matFlags.Add(flag);
                }
                else
                {
                    // Eliminar la string si está presente
                    _matFlags?.Remove(flag);
                }
            }
            
            private void UpdateColFlagValue(string colflag, bool value)
            {
                if (value)
                {
                    // Agregar la string si no está presente
                    if (_ColDisableFlag == null)
                        _ColDisableFlag = new List<string>();
                    if (!_ColDisableFlag.Contains(colflag))
                        _ColDisableFlag.Add(colflag);
                }
                else
                {
                    // Eliminar la string si está presente
                    _ColDisableFlag?.Remove(colflag);
                }
            }
            
            private void UpdateFlagsProperties()
            {
                // Actualizar cada propiedad booleana según el contenido de MatFlags
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                {
                    if (prop.Category == "MatFlag Properties" && prop.PropertyType == typeof(bool))
                    {
                        var flagName = prop.Name.Substring(2); // Eliminar "Is" del nombre de la propiedad
                        prop.SetValue(this, _matFlags != null && _matFlags.Contains(flagName));
                    }
                }
            }
            
            private void UpdateColFlagsProperties()
            {
                // Actualizar cada propiedad booleana según el contenido de ColDisableFlag
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                {
                    if (prop.Category == "ColDisableFlag Properties" && prop.PropertyType == typeof(bool))
                    {
                        var colflagName = prop.Name.Substring(2); // Eliminar "Is" del nombre de la propiedad
                        prop.SetValue(this, _ColDisableFlag != null && _ColDisableFlag.Contains(colflagName));
                    }
                }
            }

            [Category("Attributes")]
            [TypeConverter(typeof(Form1.FxPresetConverter))]
            public string FxPreset { get; set; }

            [Category("Attributes")]
            [Editor("System.Windows.Forms.Design.StringCollectionEditor, " + "System.Design, Version=2.0.0.0, " +
                    "Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
            public List<string> ColDisableFlag
            {
                get { return _ColDisableFlag; }
                set
                {
                    _ColDisableFlag = value;
                    UpdateColFlagsProperties();
                }
            }
            
            [Category("MatFlag Properties")]
            public bool IsWater
            {
                get { return _matFlags != null && _matFlags.Contains("Water"); }
                set { UpdateFlagValue("Water", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsUnridable
            { 
                get { return _matFlags != null && _matFlags.Contains("Unridable"); }
                set { UpdateFlagValue("Unridable", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsBombBlast
            { 
                get { return _matFlags != null && _matFlags.Contains("BombBlast"); }
                set { UpdateFlagValue("BombBlast", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsBombDead
            { 
                get { return _matFlags != null && _matFlags.Contains("BombDead"); }
                set { UpdateFlagValue("BombDead", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsBombCantBind
            { 
                get { return _matFlags != null && _matFlags.Contains("BombCantBind"); }
                set { UpdateFlagValue("BombCantBind", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsBombDamage
            { 
                get { return _matFlags != null && _matFlags.Contains("BombDamage"); }
                set { UpdateFlagValue("BombDamage", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsBombFizzyDirectDamage
            { 
                get { return _matFlags != null && _matFlags.Contains("BombFizzyDirectDamage"); }
                set { UpdateFlagValue("BombFizzyDirectDamage", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsUltraStampBlast
            { 
                get { return _matFlags != null && _matFlags.Contains("UltraStampBlast"); }
                set { UpdateFlagValue("UltraStampBlast", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsUltraStampDead
            { 
                get { return _matFlags != null && _matFlags.Contains("UltraStampDead"); }
                set { UpdateFlagValue("UltraStampDead", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsTripleTornadoDeviceBlast
            { 
                get { return _matFlags != null && _matFlags.Contains("TripleTornadoDeviceBlast"); }
                set { UpdateFlagValue("TripleTornadoDeviceBlast", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsTripleTornadoDeviceDead
            { 
                get { return _matFlags != null && _matFlags.Contains("TripleTornadoDeviceDead"); }
                set { UpdateFlagValue("TripleTornadoDeviceDead", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsTripleTornadoDeviceCantBind
            { 
                get { return _matFlags != null && _matFlags.Contains("TripleTornadoDeviceCantBind"); }
                set { UpdateFlagValue("TripleTornadoDeviceCantBind", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsWashtubBlast
            { 
                get { return _matFlags != null && _matFlags.Contains("WashtubBlast"); }
                set { UpdateFlagValue("WashtubBlast", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsNiceBallBombBlast
            { 
                get { return _matFlags != null && _matFlags.Contains("NiceBallBombBlast"); }
                set { UpdateFlagValue("NiceBallBombBlast", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsPlaceWithMarginWeapon
            { 
                get { return _matFlags != null && _matFlags.Contains("PlaceWithMarginWeapon"); }
                set { UpdateFlagValue("PlaceWithMarginWeapon", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsBulletCantMount
            { 
                get { return _matFlags != null && _matFlags.Contains("BulletCantMount"); }
                set { UpdateFlagValue("BulletCantMount", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsSuperHookCantBind
            { 
                get { return _matFlags != null && _matFlags.Contains("SuperHookCantBind"); }
                set { UpdateFlagValue("SuperHookCantBind", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsActivateWallaObj
            { 
                get { return _matFlags != null && _matFlags.Contains("ActivateWallaObj"); }
                set { UpdateFlagValue("ActivateWallaObj", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsDontRejectOverlapWallaObj
            { 
                get { return _matFlags != null && _matFlags.Contains("DontRejectOverlapWallaObj"); }
                set { UpdateFlagValue("DontRejectOverlapWallaObj", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsGameUnridable
            { 
                get { return _matFlags != null && _matFlags.Contains("GameUnridable"); }
                set { UpdateFlagValue("GameUnridable", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsSquidGuard
            { 
                get { return _matFlags != null && _matFlags.Contains("SquidGuard"); }
                set { UpdateFlagValue("SquidGuard", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsSlide
            { 
                get { return _matFlags != null && _matFlags.Contains("Slide"); }
                set { UpdateFlagValue("Slide", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsPlayerDead
            { 
                get { return _matFlags != null && _matFlags.Contains("PlayerDead"); }
                set { UpdateFlagValue("PlayerDead", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsPlayerUnsafe
            { 
                get { return _matFlags != null && _matFlags.Contains("PlayerUnsafe"); }
                set { UpdateFlagValue("PlayerUnsafe", value); }
            }
            [Category("MatFlag Properties")]
            public bool IsIgnoreBackFace
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoreBackFace"); }
                set { UpdateFlagValue("IgnoreBackFace", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoreDefiladeBackFace
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoreDefiladeBackFace"); }
                set { UpdateFlagValue("IgnoreDefiladeBackFace", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsFence
            { 
                get { return _matFlags != null && _matFlags.Contains("Fence"); }
                set { UpdateFlagValue("Fence", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIce
            { 
                get { return _matFlags != null && _matFlags.Contains("Ice"); }
                set { UpdateFlagValue("Ice", value); }
            }
            
            [Category("MatFlag Properties")]
            public bool IsKeepOut
            { 
                get { return _matFlags != null && _matFlags.Contains("KeepOut"); }
                set { UpdateFlagValue("KeepOut", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsFillUp
            { 
                get { return _matFlags != null && _matFlags.Contains("FillUp"); }
                set { UpdateFlagValue("FillUp", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsKebaInkCore
            { 
                get { return _matFlags != null && _matFlags.Contains("KebaInkCore"); }
                set { UpdateFlagValue("KebaInkCore", value); }
            }
            
            [Category("MatFlag Properties")]
            public bool IsKebaInk
            { 
                get { return _matFlags != null && _matFlags.Contains("KebaInk"); }
                set { UpdateFlagValue("KebaInk", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsSponge
            { 
                get { return _matFlags != null && _matFlags.Contains("Sponge"); }
                set { UpdateFlagValue("Sponge", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsCurb
            { 
                get { return _matFlags != null && _matFlags.Contains("Curb"); }
                set { UpdateFlagValue("Curb", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsDisableSurfaceVelocity
            { 
                get { return _matFlags != null && _matFlags.Contains("DisableSurfaceVelocity"); }
                set { UpdateFlagValue("DisableSurfaceVelocity", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsNoEffectHitBlower
            { 
                get { return _matFlags != null && _matFlags.Contains("NoEffectHitBlower"); }
                set { UpdateFlagValue("NoEffectHitBlower", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsCancelIgnoreBackFace
            { 
                get { return _matFlags != null && _matFlags.Contains("CancelIgnoreBackFace"); }
                set { UpdateFlagValue("CancelIgnoreBackFace", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoreGreatBarrierDroneAscend
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoreGreatBarrierDroneAscend"); }
                set { UpdateFlagValue("IgnoreGreatBarrierDroneAscend", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsCanopyKnockBackOwner
            { 
                get { return _matFlags != null && _matFlags.Contains("CanopyKnockBackOwner"); }
                set { UpdateFlagValue("CanopyKnockBackOwner", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsChargerGuide
            { 
                get { return _matFlags != null && _matFlags.Contains("ChargerGuide"); }
                set { UpdateFlagValue("ChargerGuide", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Metal
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Metal"); }
                set { UpdateFlagValue("LockerMaterial_Metal", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Wood
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Wood"); }
                set { UpdateFlagValue("LockerMaterial_Wood", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Plastic
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Plastic"); }
                set { UpdateFlagValue("LockerMaterial_Plastic", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Glass
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Glass"); }
                set { UpdateFlagValue("LockerMaterial_Glass", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Asplalt
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Asplalt"); }
                set { UpdateFlagValue("LockerMaterial_Asplalt", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Cloth
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Cloth"); }
                set { UpdateFlagValue("LockerMaterial_Cloth", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsLockerMaterial_Rubber
            { 
                get { return _matFlags != null && _matFlags.Contains("LockerMaterial_Rubber"); }
                set { UpdateFlagValue("LockerMaterial_Rubber", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsMiniMapOnly
            { 
                get { return _matFlags != null && _matFlags.Contains("MiniMapOnly"); }
                set { UpdateFlagValue("MiniMapOnly", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsCoopEnemyGround
            { 
                get { return _matFlags != null && _matFlags.Contains("CoopEnemyGround"); }
                set { UpdateFlagValue("CoopEnemyGround", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsForceColPaintYPlus
            { 
                get { return _matFlags != null && _matFlags.Contains("ForceColPaintYPlus"); }
                set { UpdateFlagValue("ForceColPaintYPlus", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsForceColPaintPaintable
            { 
                get { return _matFlags != null && _matFlags.Contains("ForceColPaintPaintable"); }
                set { UpdateFlagValue("ForceColPaintPaintable", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsForceColPaintNotPaintable
            { 
                get { return _matFlags != null && _matFlags.Contains("ForceColPaintNotPaintable"); }
                set { UpdateFlagValue("ForceColPaintNotPaintable", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsForceColPaintIndependent
            { 
                get { return _matFlags != null && _matFlags.Contains("ForceColPaintIndependent"); }
                set { UpdateFlagValue("ForceColPaintIndependent", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsForceColPaintUnite
            { 
                get { return _matFlags != null && _matFlags.Contains("ForceColPaintUnite"); }
                set { UpdateFlagValue("ForceColPaintUnite", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoredByNavMesh
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoredByNavMesh"); }
                set { UpdateFlagValue("IgnoredByNavMesh", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsBakeNavMeshExtraData
            { 
                get { return _matFlags != null && _matFlags.Contains("BakeNavMeshExtraData"); }
                set { UpdateFlagValue("BakeNavMeshExtraData", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoredByMiniMap
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoredByMiniMap"); }
                set { UpdateFlagValue("IgnoredByMiniMap", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoreAlongGround
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoreAlongGround"); }
                set { UpdateFlagValue("IgnoreAlongGround", value); }
            }

            [Category("MatFlag Properties")]
            public bool IsIgnoreByCoopFloatEnemy
            { 
                get { return _matFlags != null && _matFlags.Contains("IgnoreByCoopFloatEnemy"); }
                set { UpdateFlagValue("IgnoreByCoopFloatEnemy", value); }
            }
            
            //coldisable stuff
            
            [Category("ColDisableFlag Properties")]
            public bool IsNoHit
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("NoHit"); }
                set { UpdateColFlagValue("NoHit", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsCustomReceiver
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("CustomReceiver"); }
                set { UpdateColFlagValue("CustomReceiver", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsGameCustomReceiver
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("GameCustomReceiver"); }
                set { UpdateColFlagValue("GameCustomReceiver", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsGroundCol
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Ground"); }
                set { UpdateColFlagValue("Ground", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsWaterCol
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Water"); }
                set { UpdateColFlagValue("Water", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayer
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayer"); }
                set { UpdateColFlagValue("SplPlayer", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerChariotShieldCol
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerChariotShield"); }
                set { UpdateColFlagValue("SplPlayerChariotShield", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplCamera
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplCamera"); }
                set { UpdateColFlagValue("SplCamera", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplSakeSaucer
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSakeSaucer"); }
                set { UpdateColFlagValue("SplSakeSaucer", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplInkBullet_FriendThrough
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplInkBullet_FriendThrough"); }
                set { UpdateColFlagValue("SplInkBullet_FriendThrough", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSubstanceBullet
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSubstanceBullet"); }
                set { UpdateColFlagValue("SplSubstanceBullet", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSubstanceBullet_HitOpposite
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSubstanceBullet_HitOpposite"); }
                set { UpdateColFlagValue("SplSubstanceBullet_HitOpposite", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplVehicleSpectacle
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplVehicleSpectacle"); }
                set { UpdateColFlagValue("SplVehicleSpectacle", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplSubstanceBullet_HitBullet
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSubstanceBullet_HitBullet"); }
                set { UpdateColFlagValue("SplSubstanceBullet_HitBullet", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSubstanceBullet_Lite
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSubstanceBullet_Lite"); }
                set { UpdateColFlagValue("SplSubstanceBullet_Lite", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplRollerBody
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplRollerBody"); }
                set { UpdateColFlagValue("SplRollerBody", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplStationedWeapon
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplStationedWeapon"); }
                set { UpdateColFlagValue("SplStationedWeapon", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplInkShield
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplInkShield"); }
                set { UpdateColFlagValue("SplInkShield", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplUltraStamp
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplUltraStamp"); }
                set { UpdateColFlagValue("SplUltraStamp", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSakelienBomberBody
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSakelienBomberBody"); }
                set { UpdateColFlagValue("SplSakelienBomberBody", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplBlowerInhale
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplBlowerInhale"); }
                set { UpdateColFlagValue("SplBlowerInhale", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplBluntWeapon
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplBluntWeapon"); }
                set { UpdateColFlagValue("SplBluntWeapon", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplInkFilm
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplInkFilm"); }
                set { UpdateColFlagValue("SplInkFilm", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplInkTornado
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplInkTornado"); }
                set { UpdateColFlagValue("SplInkTornado", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplGreatBarrier
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplGreatBarrier"); }
                set { UpdateColFlagValue("SplGreatBarrier", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSaberBombGuard
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSaberBombGuard"); }
                set { UpdateColFlagValue("SplSaberBombGuard", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPaintSplash
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPaintSplash"); }
                set { UpdateColFlagValue("SplPaintSplash", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplObject
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplObject"); }
                set { UpdateColFlagValue("SplObject", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsMissionEnemy
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("MissionEnemy"); }
                set { UpdateColFlagValue("MissionEnemy", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplCoopEnemy
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplCoopEnemy"); }
                set { UpdateColFlagValue("SplCoopEnemy", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSdodrEnemy
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSdodrEnemy"); }
                set { UpdateColFlagValue("SplSdodrEnemy", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplItem
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplItem"); }
                set { UpdateColFlagValue("SplItem", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplWallaObj
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplWallaObj"); }
                set { UpdateColFlagValue("SplWallaObj", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsUnspecified
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Unspecified"); }
                set { UpdateColFlagValue("Unspecified", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsGround
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Ground"); }
                set { UpdateColFlagValue("Ground", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsGroundObject
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("GroundObject"); }
                set { UpdateColFlagValue("GroundObject", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerHuman
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerHuman"); }
                set { UpdateColFlagValue("SplPlayerHuman", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerSquid_Visible
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerSquid_Visible"); }
                set { UpdateColFlagValue("SplPlayerSquid_Visible", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerSquid_Invisible
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerSquid_Invisible"); }
                set { UpdateColFlagValue("SplPlayerSquid_Invisible", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerSquid_NoThroughFence
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerSquid_NoThroughFence"); }
                set { UpdateColFlagValue("SplPlayerSquid_NoThroughFence", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerChariot
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerChariot"); }
                set { UpdateColFlagValue("SplPlayerChariot", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerChariotShield
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerChariotShield"); }
                set { UpdateColFlagValue("SplPlayerChariotShield", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplPlayerSuperHookCheck
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplPlayerSuperHookCheck"); }
                set { UpdateColFlagValue("SplPlayerSuperHookCheck", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplSalmonBuddy
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplSalmonBuddy"); }
                set { UpdateColFlagValue("SplSalmonBuddy", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSight
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Sight"); }
                set { UpdateColFlagValue("Sight", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsEnemy
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Enemy"); }
                set { UpdateColFlagValue("Enemy", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsEnemyLarge
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("EnemyLarge"); }
                set { UpdateColFlagValue("EnemyLarge", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsMissionEnemyHide
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("MissionEnemyHide"); }
                set { UpdateColFlagValue("MissionEnemyHide", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsEnemyHide
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("EnemyHide"); }
                set { UpdateColFlagValue("EnemyHide", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsNPC
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("NPC"); }
                set { UpdateColFlagValue("NPC", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsShield
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("Shield"); }
                set { UpdateColFlagValue("Shield", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsHalfBrokenShield
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("HalfBrokenShield"); }
                set { UpdateColFlagValue("HalfBrokenShield", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsForceField
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("ForceField"); }
                set { UpdateColFlagValue("ForceField", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSubstanceBullet_Solid
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SubstanceBullet_Solid"); }
                set { UpdateColFlagValue("SubstanceBullet_Solid", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSubstanceBullet_Fragile
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SubstanceBullet_Fragile"); }
                set { UpdateColFlagValue("SubstanceBullet_Fragile", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsPlayerCustomPartStandAlone
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("PlayerCustomPartStandAlone"); }
                set { UpdateColFlagValue("PlayerCustomPartStandAlone", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplEnergyStand
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplEnergyStand"); }
                set { UpdateColFlagValue("SplEnergyStand", value); }
            }


            [Category("ColDisableFlag Properties")]
            public bool IsSplInkBullet
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplInkBullet"); }
                set { UpdateColFlagValue("SplInkBullet", value); }
            }
            
            [Category("ColDisableFlag Properties")]
            public bool IsSplBall
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplBall"); }
                set { UpdateColFlagValue("SplBall", value); }
            }

            [Category("ColDisableFlag Properties")]
            public bool IsSplOverheadBullet
            { 
                get { return _ColDisableFlag != null && _ColDisableFlag.Contains("SplOverheadBullet"); }
                set { UpdateColFlagValue("SplOverheadBullet", value); }
            }



            public Material()
            {
                MatFlags = new List<string>();
                ColDisableFlag = new List<string>();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}

import pandas as pd
from pathlib import Path

def txt_to_excel(input_path, output_path):
    # Lecture du fichier texte avec Pandas
    dataframe = pd.read_csv(input_path, encoding='UTF-8')

    # Export du DataFrame vers un fichier Excel
    dataframe.to_excel(output_path, index=False)  # index=False pour ne pas inclure l'index dans le fichier Excel
    
    print(f"'{input_path}'  '{output_path}'")

# Utilisation de la fonction pour convertir le fichier texte en Excel
txt_file = 'lecturexel1.txt'
excel_file = 'C:\\Users\\paulb\\Downloads\\lecturexel.xlsx'

p=Path(txt_file)
if p.exists():
    print("txt_file exist")
    txt_to_excel(txt_file, excel_file)

else:
    print("txt_file n existe pas")
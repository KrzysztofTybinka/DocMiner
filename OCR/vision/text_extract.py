import os
from pdf2image import convert_from_bytes
import pytesseract
from PIL import Image

pytesseract.pytesseract.tesseract_cmd = os.getenv('TESSERACT_PATH', '/usr/bin/tesseract')

def extract_text_from_pdf(pdf_file):

    images = convert_from_bytes(pdf_file.read())  # Convert PDF to images
    text = ''

    for image in images:

        text += pytesseract.image_to_string(image)  # Convert image to string
    return text
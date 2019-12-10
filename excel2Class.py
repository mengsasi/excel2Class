#-*- coding: UTF-8 -*-

import os
import sys
import codecs
import xlrd #http://pypi.python.org/pypi/xlrd

templateClass = "Template.txt"
templateProp = "TemplateProperty.txt"

def ReadLine (name):
    arr = []
    f = codecs.open(name,"r","utf-8")
    line = f.readline()
    while line:
        arr.append(line)
        line = f.readline()
    f.close()
    return arr

def GeneratorPropArr(table, tableName):
    ncols = table.ncols
    propArr = []
    hasID = False
    for c in range(ncols):
        prop = {}
        propName = table.cell_value(0,c)
        propType = table.cell_value(1,c)
        
        if propType == "ignore" or propType == "json": #json自行解析
            continue

        if(propName == "_id"):
            prop["classProp"] = "ID"
            hasID = True
        else:
            pre = propName[0:1]
            suf = propName[1:len(propName)]
            prop["classProp"] = pre.upper() + suf
        prop["jsonProp"] = propName
        ty = ""
        if propType == "string":
            ty = "string"
        elif propType == "number":
            ty = "float"
        elif propType == "integer":
            ty = "int"
        elif propType == "bool":
            ty = "bool"
        prop["classPropType"] = ty
        propArr.append(prop)
    if(not hasID):
        print(tableName + "has no _id") #需要_id列
    return propArr

def ReplaceProp(prop):
    strTemp = ""
    strTemp += "\t\t"
    strTemp += "[JsonParser( \"" + prop["jsonProp"] + "\" )]"
    strTemp += "\n\t\t"
    strTemp += "public " + prop["classPropType"] + " " + prop["classProp"] + " { get; set; }"
    
    '''lines = ReadLine(templateProp)
    for line in lines:
        strTemp += "\t\t"
        line = line.replace("|jsonProp|", prop["jsonProp"])
        line = line.replace("|classPropType|", prop["classPropType"])
        line = line.replace("|classProp|", prop["classProp"])
        strTemp += line'''
    return strTemp

def GeneratorProps(table, tableName):
    strTemp = ""
    l = 0
    arr = GeneratorPropArr(table, tableName)
    for prop in arr:
        if(l > 0):
            strTemp += "\n\n"
        strTemp += ReplaceProp(prop)
        l += 1
    return strTemp

def table2class(table, tableName):
    className = tableName + "Configs"
    confPath = "configs/" + className + ".cs"
    dir = os.path.dirname(confPath)
    if dir and not os.path.exists(dir):
        os.makedirs(dir)
    f = codecs.open(confPath,"w","utf-8")
    template = codecs.open(templateClass,"r","utf-8")
    strTemp = template.read()
    propStr = GeneratorProps(table, tableName)
	
    repTable = {}
    repTable["|configName|"] = tableName + "Config"
    repTable["|configsName|"] = className
    repTable["|tableName|"] = tableName
    repTable["|configDict|"] = tableName + "Dict"
    repTable["||"] = propStr
	
    for key in repTable:
        strTemp = strTemp.replace(key,repTable[key])
   
    '''strTemp = strTemp.replace("|configName|",tableName + "Config")
    strTemp = strTemp.replace("|configsName|",className)
    strTemp = strTemp.replace("|tableName|",tableName)
    strTemp = strTemp.replace("|configDict|",tableName + "Dict")
    strTemp = strTemp.replace("||",propStr)'''
    
    f.write(strTemp)
    f.close()
    template.close()

if __name__ == '__main__':
    if len(sys.argv) < 2:
        print('Usage: %s <excel_file>' % sys.argv[0])
        sys.exit(1)

    print("handle file: %s" % sys.argv[1])
	
    excelFileName = sys.argv[1]
    data = xlrd.open_workbook(excelFileName)
    allSheetNames = data.sheet_names()
    for name in allSheetNames:
        exports = name.split("_")
        if len(exports) > 1:
            if str(exports[1]) == "noexport":
                continue
        table = data.sheet_by_name(name)
        table2class(table, name)

    print("All OK")

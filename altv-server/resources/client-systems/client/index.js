/// <reference types = "@altv/types-client">
/// <reference types = "@altv/types-natives">
/// <reference types = "@altv/types-shared">
import * as alt from 'alt-client';
import * as native from 'natives';
import * as shared from 'alt-shared';
import * as NativeUI from 'includes/nativeui/NativeUI.mjs'

const Clothes = JSON.parse(alt.File.read('client/pedComponentVariations.json'));

const DEFAULT_SPAWN_POSITION = new shared.Vector3(0, 0, 75);

alt.onServer('freezePlayer', (toggle) => {
  const lPlayer = alt.Player.local.scriptID;
  native.freezeEntityPosition(lPlayer, toggle);
  //alt.toggleGameControls(!toggle);
});

let loginHud;
// NativeUI : faction cmd
alt.onServer('Client:UIMenu:Create', (callbackEvent, targetId, title, description, json) => {
  let itemList = JSON.parse(json);
  console.log(itemList);
  const ui = new NativeUI.Menu(title, description, new NativeUI.Point(50,50));
  ui.Open();

  for (let i = 0; i < itemList.length; ++i)
  {
    let item = null;
    if (itemList[i].Current !== null)
    {
      item = new NativeUI.UIMenuListItem(
        itemList[i].Name,
        itemList[i].Description,
        new NativeUI.ItemsCollection(itemList[i].Items),
        itemList[i].Current
      );
    }
    else
    {
      item = new NativeUI.UIMenuListItem(
        itemList[i].Name,
        itemList[i].Description,
        new NativeUI.ItemsCollection(itemList[i].Items)
      );
    }
    ui.AddItem(item);
  }

  ui.ItemSelect.on(item => {
  	if (item instanceof NativeUI.UIMenuListItem) {
      alt.log(item.Text, item.Index, item.SelectedItem.DisplayText, item.SelectedItem.Data);
      alt.emitServer(callbackEvent, targetId, item.Text, item.Index);
      ui.Close();
   	}
  });

  ui.MenuClose.on(() => {
    alt.emitServer('Server:UIMenu:Close');
  });
});

const ShapesGroup = {
  male: [
      { name: 'Benjamin', id: 0, image: 'static/images/parents/parent_0.png' },
      { name: 'Daniel', id: 1, image: 'static/images/parents/parent_1.png' },
      { name: 'Joshua', id: 2, image: 'static/images/parents/parent_2.png' },
      { name: 'Noah', id: 3, image: 'static/images/parents/parent_3.png' },
      { name: 'Andrew', id: 4, image: 'static/images/parents/parent_4.png' },
      { name: 'Juan', id: 5, image: 'static/images/parents/parent_5.png' },
      { name: 'Alex', id: 6, image: 'static/images/parents/parent_6.png' },
      { name: 'Isaac', id: 7, image: 'static/images/parents/parent_7.png' },
      { name: 'Evan', id: 8, image: 'static/images/parents/parent_8.png' },
      { name: 'Ethan', id: 9, image: 'static/images/parents/parent_9.png' },
      { name: 'Vincent', id: 10, image: 'static/images/parents/parent_10.png' },
      { name: 'Angel', id: 11, image: 'static/images/parents/parent_11.png' },
      { name: 'Diego', id: 12, image: 'static/images/parents/parent_12.png' },
      { name: 'Adrian', id: 13, image: 'static/images/parents/parent_13.png' },
      { name: 'Gabriel', id: 14, image: 'static/images/parents/parent_14.png' },
      { name: 'Michael', id: 15, image: 'static/images/parents/parent_15.png' },
      { name: 'Santiago', id: 16, image: 'static/images/parents/parent_16.png' },
      { name: 'Kevin', id: 17, image: 'static/images/parents/parent_17.png' },
      { name: 'Louis', id: 18, image: 'static/images/parents/parent_18.png' },
      { name: 'Samuel', id: 19, image: 'static/images/parents/parent_19.png' },
      { name: 'Anthony', id: 20, image: 'static/images/parents/parent_20.png' },
      { name: 'Claude', id: 42, image: 'static/images/parents/parent_42.png' },
      { name: 'Niko', id: 43, image: 'static/images/parents/parent_43.png' },
      { name: 'John', id: 44, image: 'static/images/parents/parent_44.png' },
  ],
  female: [
      { name: 'Hannah', id: 21, image: 'static/images/parents/parent_21.png' },
      { name: 'Audrey', id: 22, image: 'static/images/parents/parent_22.png' },
      { name: 'Jasmine', id: 23, image: 'static/images/parents/parent_23.png' },
      { name: 'Giselle', id: 24, image: 'static/images/parents/parent_24.png' },
      { name: 'Amelia', id: 25, image: 'static/images/parents/parent_25.png' },
      { name: 'Isabella', id: 26, image: 'static/images/parents/parent_26.png' },
      { name: 'Zoe', id: 27, image: 'static/images/parents/parent_27.png' },
      { name: 'Ava', id: 28, image: 'static/images/parents/parent_28.png' },
      { name: 'Camila', id: 29, image: 'static/images/parents/parent_29.png' },
      { name: 'Violet', id: 30, image: 'static/images/parents/parent_30.png' },
      { name: 'Sophia', id: 31, image: 'static/images/parents/parent_31.png' },
      { name: 'Evelyn', id: 32, image: 'static/images/parents/parent_32.png' },
      { name: 'Nicole', id: 33, image: 'static/images/parents/parent_33.png' },
      { name: 'Ashley', id: 34, image: 'static/images/parents/parent_34.png' },
      { name: 'Grace', id: 35, image: 'static/images/parents/parent_35.png' },
      { name: 'Brianna', id: 36, image: 'static/images/parents/parent_36.png' },
      { name: 'Natalie', id: 37, image: 'static/images/parents/parent_37.png' },
      { name: 'Olivia', id: 38, image: 'static/images/parents/parent_38.png' },
      { name: 'Elizabeth', id: 39, image: 'static/images/parents/parent_39.png' },
      { name: 'Charlotte', id: 40, image: 'static/images/parents/parent_40.png' },
      { name: 'Emma', id: 41, image: 'static/images/parents/parent_41.png' },
      { name: 'Misty', id: 45, image: 'static/images/parents/parent_45.png' },
  ],
};

const StructureGroup = [
  {
      label: 'Ширина носа',
      key: 'nosewidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 0
  },
  {
      label: 'Высота носа',
      key: 'noseheight',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 1
  },
  {
      label: 'Длина носа',
      key: 'noselength',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 2
  },
  {
      label: 'Переносица',
      key: 'nosebridge',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 3
  },
  {
      label: 'Кончик носа',
      key: 'nosetip',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 4
  },
  {
      label: 'Высота переносицы',
      key: 'nosebridgeshaft',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 5
  },
  {
      label: 'Высота бровей',
      key: 'browheight',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 6
  },
  {
      label: 'Ширина бровей',
      key: 'browwidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 7
  },
  {
      label: 'Высота скул',
      key: 'cheekboneheight',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 8
  },
  {
      label: 'Ширина скул',
      key: 'cheekbonewidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 9
  },
  {
      label: 'Ширинра щек',
      key: 'cheekwidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 10
  },
  {
      label: 'Веки',
      key: 'eyelids',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 11
  },
  {
      label: 'Губы',
      key: 'lips',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 12
  },
  {
      label: 'Ширина челюсти',
      key: 'jawwidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 13
  },
  {
      label: 'Высота челюсти',
      key: 'jawheight',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 14
  },
  {
      label: 'Длина подбодка',
      key: 'chinlength',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 15
  },
  {
      label: 'Положение подбородка',
      key: 'chinposition',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 16
  },
  {
      label: 'Ширина подбородка',
      key: 'chinwidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 17
  },
  {
      label: 'Форма подбородка',
      key: 'chinshape',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 18
  },
  {
      label: 'Ширина шеи',
      key: 'neckwidth',
      value: 0,
      min: -1,
      max: 1,
      increment: 0.1,
      id: 19
  }
]

const GENDER = {MALE : 0, FEMALE : 1};

let EditorSettings = {
  HeadBlend : {
    shapeFemaleIdx : 0,
    shapeMaleIdx : 0,
    skinFemaleIdx : 0,
    shapeMixIdx : 10,
    skinMixIdx : 0
  },
  Hair : {
    DrawableIdx : 0,
    TextureIdx : 0,
  }
};

let COMPONENT_NAMES = {
  0 : 'Лицо',
  1 : 'Маска',
  2 : 'Волосы',
  3 : 'Торс',
  4 : 'Ноги',
  5 : 'Парашют/Рюкзак',
  6 : 'Обувь',
  7 : 'Аксессуар',
  8 : 'Майка',
  9 : 'Кевлар',
  10 : 'Значок',
  11 : 'Торс 2'
}

function generateArray(lowerBound, upperBound, step = 1) {
  if (typeof step === 'number' && !Number.isInteger(step)) {
    return Array.from({length: (upperBound-lowerBound)/step + 1}, (_, i) => Math.round((lowerBound + i * step) * 10) / 10);
  }
  return Array.from({length: (upperBound-lowerBound)/step + 1}, (_, i) => lowerBound + i * step);
}

/* Пункт меню "Наследственность" */
function InitInheritanceItem(menu, clonedPed) {
  const AvailableSkins = generateArray(0, 45, 1);
  const AvailableMix = generateArray(-1.0, 1.0, 0.1);

  const itemInheritance = new NativeUI.UIMenuItem('Наследственность', 'Выбор внешности родителей');
  itemInheritance.Enabled = true;
  menu.AddItem(itemInheritance)

    const HeadBlend = EditorSettings.HeadBlend;

  const submenu = new NativeUI.Menu('Наследственность', 'Выбор внешности родителей', new NativeUI.Point(50,50));
  const itemMale = new NativeUI.UIMenuListItem('Отец', 'Выберите внешность отца', new NativeUI.ItemsCollection(ShapesGroup.male.map(item => item.name)), HeadBlend.shapeMaleIdx);
  const itemFemale = new NativeUI.UIMenuListItem('Мать', 'Выберите внешность матери', new NativeUI.ItemsCollection(ShapesGroup.female.map(item => item.name)), HeadBlend.shapeFemaleIdx);
  const itemSkin = new NativeUI.UIMenuSliderItem('Цвет кожи', AvailableSkins.map(num => num.toString()), HeadBlend.skinFemaleIdx, 'Выберите цвет кожи', true);
  const itemShapeMix = new NativeUI.UIMenuSliderItem('Схожесть', AvailableMix.map(num => num.toString()), HeadBlend.shapeMixIdx, '<< Мать       Отец >>', true);
  const itemSkinMix = new NativeUI.UIMenuSliderItem('Насыщенность', AvailableMix.map(num => num.toString()), HeadBlend.skinMixIdx, '<< Темнее       Светлее >>', true);
    
  submenu.AddItem(itemMale);
  submenu.AddItem(itemFemale);
  submenu.AddItem(itemShapeMix)
  submenu.AddItem(itemSkin);
  submenu.AddItem(itemSkinMix);

  menu.BindMenuToItem(submenu, itemInheritance);

  const ApplyPedHeadBlendData = ()=> {
    native.setPedHeadBlendData(clonedPed,
      ShapesGroup.female[HeadBlend.shapeFemaleIdx].id, ShapesGroup.male[HeadBlend.shapeMaleIdx].id, 0,
      AvailableSkins[HeadBlend.skinFemaleIdx], 0, 0,
      AvailableMix[HeadBlend.shapeMixIdx], AvailableMix[HeadBlend.skinMixIdx], 0.0,
      false
    );
  };

  /// Проверка измененности в пункте меню "Наследственность"
  submenu.ListChange.on((item, index) => {
    // Если это внешность отца
    if (item === itemMale) {
      HeadBlend.shapeMaleIdx = +index;
    }
    // Если это внешность матери
    else if (item === itemFemale) {
      HeadBlend.shapeFemaleIdx = +index;
    }
    ApplyPedHeadBlendData();
  });

  submenu.SliderChange.on((item, index, value) => {
    // Если это цвет кожи
    if (item === itemSkin) {
      HeadBlend.skinFemaleIdx = +index;
    }
    // Если это схожесть внешности
    else if (item === itemShapeMix) {
      HeadBlend.shapeMixIdx = +index;
    }
    // Если это насыщеность кожи
    else if (item === itemSkinMix) {
        HeadBlend.skinMixIdx = +index;
    }
    ApplyPedHeadBlendData();
  });
}

/* Волосы */
function InitHairItem(menu, clonedPed) {
  alt.log(Clothes['0']['2'].length);
  const Hair = EditorSettings.Hair;

  function ApplyHairStyle()
  {
    const hairComponents = Clothes['0']['2'];
    native.setPedComponentVariation(clonedPed, 2, hairComponents[Hair.DrawableIdx]['drawableId'], hairComponents[Hair.DrawableIdx]['textureId'][Hair.TextureIdx], 0);
  }

  const itemHair = new NativeUI.UIMenuItem('Волосы', 'Выбор волос');
  //itemHair.Enabled = true;
  menu.AddItem(itemHair)

  const submenu = new NativeUI.Menu('Волосы', 'Выбор волос', new NativeUI.Point(50,50));
  let subItemHair = new NativeUI.UIMenuListItem('Прическа', 'Выбор прически', new NativeUI.ItemsCollection(generateArray(0, Clothes['0']['2'].length).map(num => num.toString())), Hair.DrawableIdx);
  let subItemHairStyle = new NativeUI.UIMenuListItem('Стиль', 'Выбор стиля', new NativeUI.ItemsColletion(generateArray(0, Clothes['0']['2'][Hair.ComponentIdx]['textureId'].length).map(num => num.toString())), Hair.TextureIdx);
  
  submenu.AddItem(subItemHair);
  submenu.AddItem(subItemHairStyle);

  menu.BindMenuToItem(submenu, itemHair);

  submenu.ListChange.on((item, index) => {
    if (item === subItemHair) {
      Hair.DrawableIdx = +index;
      Hair.TextureIdx = 0;
      subItemHairStyle = new NativeUI.UIMenuListItem('Стиль', 'Выбор стиля', new NativeUI.ItemsColletion(generateArray(0, Clothes['0']['2'][Hair.ComponentIdx]['textureId'].length).map(num => num.toString())), Hair.TextureIdx);
      ApplyHairStyle()
    } else if (item === subItemHairStyle) {
      Hair.TextureIdx = +index;
      ApplyHairStyle()
    }
  });
}

alt.onServer('UiMenu.Editor.Open', (gender) => {
  alt.log('UiMenu.Editor.Open');
  const clonedPed = native.clonePed(alt.Player.local, false, false, false);
  native.setPedHeadBlendData(clonedPed, 0, 0, 0, 0, 0, 0, 0.0, 0.0, 0.0, false);
  native.freezeEntityPosition(clonedPed, true);
  const ui = new NativeUI.Menu('Редактор персонажа', 'Создание персонажа', new NativeUI.Point(50,50));
  ui.Open();

  InitInheritanceItem(ui, clonedPed);
  InitHairItem(ui, clonedPed);
  
  // ui.MenuClose.on(() =>{
  //   ui.Open();
  // });
});

alt.onServer('Player:CloseLoginHud', () => {
  alt.showCursor(false);
  alt.toggleGameControls(true);
  alt.toggleVoiceControls(true);

  if (loginHud)
  {
    loginHud.destroy();
  }
});

alt.onServer('LoginSystem:Error', (key) => {
  loginHud.emit('Player:ShowError', key);
});

alt.on('connectionComplete', async () => {
  native.switchToMultiFirstpart(alt.Player.local.scriptID, 0, 1);
  native.doScreenFadeOut(10);
  await shared.Utils.waitFor(()=>{
    return native.isScreenFadedOut() === true;
  }, 1000);
  //await shared.Utils.loadMapArea(DEFAULT_SPAWN_POSITION, 50, 5000);

  loginHud = new alt.WebView("http://resource/html-login/index.html");
  loginHud.focus();
  alt.showCursor(true);
  alt.toggleGameControls(false);
  alt.toggleVoiceControls(false);

  loginHud.on('Player:Login', (email, password) => {
    console.log('Client -> Player:Login')
    alt.emitServer('Event:Login', email, password);
  });
  
  loginHud.on('Player:Register', (email, password, nickname, sex) => {
    console.log('Client -> Player:Register')
    alt.emitServer('Event:Register', email, password, nickname, sex);
  });

  alt.loadDefaultIpls();
});

alt.onServer('Client:BeforeSpawn', async (x, y, z, timeMs) => {
  /*native.doScreenFadeOut(timeMs);
  await shared.Utils.waitFor(()=>{
    return native.isScreenFadedOut() === true;
  });*/
  await shared.Utils.loadMapArea(new shared.Vector3(x, y, z), 50, 5000);
  native.switchToMultiFirstpart(alt.Player.local.scriptID, 0, 1);
});

const sleep = (milliseconds) => {
  return new Promise(resolve => setTimeout(resolve, milliseconds))
}

alt.onServer('Client:BeforeChangePosition', async(x, y, z) => {
  native.freezeEntityPosition(alt.Player.local.scriptID, true);
  await shared.Utils.loadMapArea(new shared.Vector3(x, y, z), 50, 5000);
});

alt.onServer('Client:PositionChanged', async () => {
  await sleep(500);
  native.freezeEntityPosition(alt.Player.local.scriptID, false);
});

alt.on('spawned', async () => {
  native.freezeEntityPosition(alt.Player.local.scriptID, true);
  native.shutdownLoadingScreen();
  if (native.isScreenFadedOut() === true) {
    native.doScreenFadeIn(500);
    await shared.Utils.waitFor(()=>{
      return native.isScreenFadedIn() === true;
    }, 2000);
  }
  native.switchToMultiSecondpart(alt.Player.local.scriptID);
  native.freezeEntityPosition(alt.Player.local.scriptID, false);
});

alt.onServer('Client:TeleportToWayPoint', async () => {
  const waypoint = native.getFirstBlipInfoId(8);
  let groundCoord = null;
  if (native.doesBlipExist(waypoint))
  {
    const blipCoord = native.getBlipInfoIdCoord(waypoint);
    alt.FocusData.overrideFocus(blipCoord);
    let startCoord = new alt.Vector3(blipCoord.x, blipCoord.y, 1500.0);
    let endCoord = startCoord;
    try {
      alt.log('waitFor =>')
      await shared.Utils.waitFor(() => {
        alt.log('waitFor start');
        //startCoord = startCoord.sub(0, 0, 200.0);
        endCoord = endCoord.sub(0, 0, 200.0);
        alt.FocusData.overrideFocus(endCoord);
        if (endCoord.z < -500)
        {
          throw new Error('Failed to get ground position!');
        }
        const shapeTestHandle = native.startExpensiveSynchronousShapeTestLosProbe(startCoord.x, startCoord.y, startCoord.z, endCoord.x, endCoord.y, endCoord.z, 0xFFFFFFFF, alt.Player.local, 0);
        const [shapeRetval, shapeHit, shapeEndCoord, , ] = native.getShapeTestResult(shapeTestHandle);
        alt.log(`shapeRetval = ${shapeRetval} | shapeHit = ${shapeHit} | shapeEndCoords = ${shapeEndCoord}`);
        if (shapeHit === true)
        {
          alt.log('CASE 2');
          groundCoord = shapeEndCoord;
          return true;
        }
        return false;
      }, 5000);
    } catch { }
    alt.FocusData.clearFocus();
    if (groundCoord === null)
    {
      groundCoord = blipCoord;
    }
    await shared.Utils.loadMapArea(groundCoord, 50, 5000);
    alt.emitServer('Server:TeleportToWayPoint', {x: groundCoord.x, y: groundCoord.y, z: groundCoord.z + 2.0});
  }
});

alt.onServer('Client:SwitchOutPlayer', () =>
{
  native.switchToMultiFirstpart(alt.Player.local.scriptID, 0, 1);
  //alt.emitServer('Server:SpawnPlayer');
});

alt.onServer('Client:SwitchToPlayer', () =>
{
  native.switchToMultiSecondpart(alt.Player.local.scriptID);
  //alt.emitServer('Server:SpawnPlayer');
});
/*alt.onServer("Client:FadeOutBeforeSpawn", async (x, y, z) => {
  native.doScreenFadeOut(500);
  await shared.Utils.waitFor(()=>{
    return native.isScreenFadedOut() === true;
  });
  native.requestCollisionAtCoord(x, y, z);
  native.clearPedTasksImmediately(alt.Player.local.scriptID);
  await shared.Utils.waitFor(()=>{
    return native.hasCollisionLoadedAroundEntity(alt.Player.local.scriptID) === true;
  }, 5000);
  alt.emitServer('Spawn');
  native.shutdownLoadingScreen();
  if (native.isScreenFadedOut() === true) {
    native.doScreenFadeIn(500);
    await shared.Utils.waitFor(()=>{
      return native.isScreenFadedIn() === true;
    });
  }
});*/